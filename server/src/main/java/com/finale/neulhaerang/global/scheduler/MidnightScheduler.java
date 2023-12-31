package com.finale.neulhaerang.global.scheduler;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.List;

import javax.persistence.EntityManager;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.ApplicationEventPublisher;
import org.springframework.scheduling.annotation.Async;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Component;
import org.springframework.transaction.annotation.Transactional;

import com.finale.neulhaerang.domain.letter.dto.common.ChatGptDto;
import com.finale.neulhaerang.domain.letter.dto.request.LetterReqDto;
import com.finale.neulhaerang.domain.letter.dto.response.LetterResDto;
import com.finale.neulhaerang.domain.letter.entity.Letter;
import com.finale.neulhaerang.domain.letter.feignclient.LetterFeignClient;
import com.finale.neulhaerang.domain.letter.repository.LetterRepository;
import com.finale.neulhaerang.domain.member.document.StatRecord;
import com.finale.neulhaerang.domain.member.dto.request.StatRecordReqDto;
import com.finale.neulhaerang.domain.member.entity.Member;
import com.finale.neulhaerang.domain.member.repository.MemberRepository;
import com.finale.neulhaerang.domain.member.repository.MemberStatRepository;
import com.finale.neulhaerang.domain.routine.entity.DailyRoutine;
import com.finale.neulhaerang.domain.routine.entity.Routine;
import com.finale.neulhaerang.domain.routine.entity.StatType;
import com.finale.neulhaerang.domain.routine.repository.DailyRoutineRepository;
import com.finale.neulhaerang.domain.routine.repository.RoutineRepository;
import com.finale.neulhaerang.domain.todo.entity.Todo;
import com.finale.neulhaerang.domain.todo.repository.TodoRepository;
import com.finale.neulhaerang.global.event.LetterEvent;
import com.finale.neulhaerang.global.event.StatEvent;
import com.finale.neulhaerang.global.event.WeatherEvent;
import com.finale.neulhaerang.global.notification.service.NotificationService;

import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;

@Component
@RequiredArgsConstructor
@Slf4j
public class MidnightScheduler {

	private final RoutineRepository routineRepository;
	private final DailyRoutineRepository dailyRoutineRepository;
	private final TodoRepository todoRepository;
	private final MemberRepository memberRepository;
	private final MemberStatRepository memberStatRepository;
	private final LetterRepository letterRepository;
	private final LetterFeignClient letterFeignClient;
	private final NotificationService notificationService;
	private final ApplicationEventPublisher publisher;
	private final EntityManager entityManager;

	@Value("${gpt.key}")
	private String gptKey;

	@Async
	@Transactional
	@Scheduled(cron = "${schedules.cron.daily-routine}", zone = "Asia/Seoul")
	public void createDailiyRoutineTrigger() {
		createDailyRoutine(LocalDate.now());
	}

	@Async
	@Scheduled(cron = "${schedules.cron.daily-routine}", zone = "Asia/Seoul")
	public void createLetterTrigger() {
		log.info("---------- 자정 스케줄러 : 편지를 전송합니다 ----------");
		List<Member> memberList = memberRepository.findAllByWithdrawalDateIsNull();
		for (Member member : memberList) {
			createLetter(member, LocalDate.now().minusDays(1));
		}
	}

	@Async
	@Scheduled(cron = "${schedules.cron.daily-routine}", zone = "Asia/Seoul")
	public void createModifyStatTrigger() {
		List<Member> memberList = memberRepository.findAllByWithdrawalDateIsNull();
		modifyStat(memberList, LocalDate.now().minusDays(1));
	}

	void createDailyRoutine(LocalDate date) {
		log.info("---------- 자정 스케줄러 : daily routine을 생성합니다 ----------");
		StringBuilder dayOfVaule = new StringBuilder("_______");
		int dayOfWeekValue = date.getDayOfWeek().getValue() - 1;
		dayOfVaule.setCharAt(dayOfWeekValue, '1');
		List<Routine> routinesOfDay = routineRepository.findRoutinesByDayOfValue(dayOfVaule.toString(), date);

		routinesOfDay.forEach(r -> {
			dailyRoutineRepository.save(DailyRoutine.create(r, date));
		});
	}

	void modifyStat(List<Member> memberList, LocalDate date) {
		for (Member member : memberList) {
			// 그 날 완료한 투두 STAT 상승
			log.info("---------- 자정 스케줄러 : 투두 스탯을 증가시킵니다 ----------");
			List<Todo> doneTodoList = modifyStatByTodo(date, member);

			// 그 날 완료한 루틴 STAT 상승
			log.info("---------- 자정 스케줄러 : 루틴 스탯을 증가시킵니다 ----------");
			List<DailyRoutine> doneDailyRoutineList = modifyStatByRoutine(date, member);

			// 투두, 루틴 완료 갯수에 따라서 피곤도 STAT 증가
			log.info("---------- 자정 스케줄러 : 피곤도 스탯을 증가시킵니다 ----------");
			int totalDone = modifyTirednessByTodoAndRoutine(date, member, doneTodoList, doneDailyRoutineList);

			// 투두, 루틴 완료 비율에 따라서 나태도 STAT 증가
			log.info("---------- 자정 스케줄러 : 나태도 스탯을 증가시킵니다 ----------");
			modifyIndolenceByTodoAndRoutine(date, member, totalDone);

			// 스탯 칭호 발급 이벤트 호출
			publisher.publishEvent(new StatEvent(member));
			// 날씨 칭호 발급 이벤트 호출
			publisher.publishEvent(new WeatherEvent(member));
		}
	}

	private List<Todo> modifyStatByTodo(LocalDate date, Member member) {
		List<Todo> doneTodoList = todoRepository.findTodosByMemberAndStatusIsFalseAndCheckIsTrueAndTodoDateIsBetween(
			member, date.atStartOfDay(), date.atTime(LocalTime.MAX)
		);
		for (Todo todo : doneTodoList) {
			createStatRecord(todo.getContent(), todo.getTodoDate(), todo.getStatType(), 2, member);
		}
		return doneTodoList;
	}

	private List<DailyRoutine> modifyStatByRoutine(LocalDate date, Member member) {
		List<DailyRoutine> doneDailyRoutineList = dailyRoutineRepository.findDailyRoutinesByRoutineDateAndRoutine_MemberAndStatusIsFalseAndCheckIsTrue(
			date, member);
		for (DailyRoutine dailyRoutine : doneDailyRoutineList) {
			createStatRecord(
				dailyRoutine.getRoutine().getContent(), dailyRoutine.getRoutineDate().atStartOfDay(),
				dailyRoutine.getRoutine().getStatType(), 5, member
			);
		}
		return doneDailyRoutineList;
	}

	private int modifyTirednessByTodoAndRoutine(LocalDate date, Member member, List<Todo> doneTodoList,
		List<DailyRoutine> doneDailyRoutineList) {
		int totalDone = doneTodoList.size() + doneDailyRoutineList.size();
		if (totalDone >= 30) {
			createStatRecord("완료한 일이 30개 이상", date.atTime(23, 59), StatType.피곤도, 50, member);
		} else if (totalDone >= 20) {
			createStatRecord("완료한 일이 20개 이상", date.atTime(23, 59), StatType.피곤도, 30, member);
		} else if (totalDone >= 10) {
			createStatRecord("완료한 일이 10개 이상", date.atTime(23, 59), StatType.피곤도, 10, member);
		}
		return totalDone;
	}

	private void modifyIndolenceByTodoAndRoutine(LocalDate date, Member member, double totalDone) {
		int totalTodo = todoRepository.findTodosByMemberAndStatusIsFalseAndTodoDateIsBetween(member,
			date.atStartOfDay(), date.atTime(LocalTime.MAX)).size();
		int totalRoutine = dailyRoutineRepository.findDailyRoutinesByRoutineDateAndRoutine_MemberAndStatusIsFalse(date,
			member).size();
		double ratio = totalDone / (totalTodo + totalRoutine);
		if (ratio < 0.2) {
			createStatRecord("80% 이상 완료하지 못했습니다.", date.atTime(23, 59), StatType.나태도, 50, member);
		} else if (ratio < 0.5) {
			createStatRecord("50% 이상 완료하지 못했습니다.", date.atTime(23, 59), StatType.나태도, 30, member);
		} else if (ratio < 0.7) {
			createStatRecord("30% 이상 완료하지 못했습니다.", date.atTime(23, 59), StatType.나태도, 10, member);
		}
	}

	public void createLetter(Member member, LocalDate date) {
		log.info("-----------  편지를 " + member.getNickname() + "님께 전송합니다 --------");
		String CONTENT_TYPE = "application/x-www-form-urlencoded; charset=UTF-8";

		String reqMessage = createReqMessage(member, date);
		List<ChatGptDto> content = new ArrayList<>();
		content.add(ChatGptDto.from(reqMessage));

		LetterReqDto letterReqDto = LetterReqDto.from(content);
		try {
			LetterResDto letterResDto = letterFeignClient.getGPTResponse(CONTENT_TYPE, "Bearer " + gptKey,
				letterReqDto);
			Letter letter = Letter.create(member, letterResDto.getChoices().get(0).getMessage().getContent(), date);
			letterRepository.save(letter);
			log.info("-----------  편지를 " + member.getNickname() + "님께 전송했습니다 --------");
			publisher.publishEvent(new LetterEvent(member));
		} catch (Exception e) {
			log.error("-----------  편지를 " + member.getNickname() + "님께 전송할 때 에러가 발생했습니다 --------");
		}

	}

	public String createReqMessage(Member member, LocalDate date) {
		List<Todo> doneTodoList = todoRepository.findTodosByMemberAndStatusIsFalseAndCheckIsTrueAndTodoDateIsBetween(
			member, date.atStartOfDay(), date.atTime(LocalTime.MAX)
		);
		List<DailyRoutine> doneDailyRoutineList = dailyRoutineRepository.findDailyRoutinesByRoutineDateAndRoutine_MemberAndStatusIsFalseAndCheckIsTrue(
			date, member);

		StringBuilder reqMessage = new StringBuilder("너의 이름은 해랑이야. 내 이름은 " + member.getNickname() + "이야.");
		if(doneTodoList.size()+doneDailyRoutineList.size()==0){
			reqMessage.append("나는 어제 아무 일도 하지 않았어. 나의 내일을 응원하는 ");
		}
		else {
			reqMessage.append("내가 어제 한 일은 다음과 같아.");

			for (Todo todo : doneTodoList) {
				reqMessage.append(todo.getContent()).append(",");
			}
			for (DailyRoutine dailyRoutine : doneDailyRoutineList) {
				reqMessage.append(dailyRoutine.getRoutine().getContent()).append(",");
			}
			reqMessage.append("위의 일을 바탕으로 긍정적이고 동기부여를 주는 ");
		}
		reqMessage.append("편지를 작성해줘. 반말로 이모지를 사용해서 귀엽게 부탁해. 시작은 TO.")
			.append(member.getNickname())
			.append("으로 시작하고 마무리할 때는 FROM.해랑으로 해줘");
		return reqMessage.toString();
	}

	void createStatRecord(String content, LocalDateTime dateTime, StatType statType, int weight, Member member) {
		StatRecordReqDto statRecordReqDto = StatRecordReqDto.of(
			content, dateTime, statType, weight
		);
		memberStatRepository.save(StatRecord.of(statRecordReqDto, member.getId()));
	}
}
