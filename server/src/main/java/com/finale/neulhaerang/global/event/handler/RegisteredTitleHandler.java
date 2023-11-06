package com.finale.neulhaerang.global.event.handler;

import java.util.Optional;

import org.springframework.context.event.EventListener;
import org.springframework.scheduling.annotation.Async;
import org.springframework.stereotype.Component;

import com.finale.neulhaerang.domain.member.entity.Member;
import com.finale.neulhaerang.domain.title.entity.EarnedTitle;
import com.finale.neulhaerang.domain.title.entity.Title;
import com.finale.neulhaerang.domain.title.repository.EarnedTitleRepository;
import com.finale.neulhaerang.domain.title.repository.TitleRepository;
import com.finale.neulhaerang.global.event.RegisteredEvent;
import com.finale.neulhaerang.global.exception.title.NotExistTitleException;

import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;

@Component
@RequiredArgsConstructor
@Slf4j
public class RegisteredTitleHandler {
	private final TitleRepository titleRepository;
	private final EarnedTitleRepository earnedTitleRepository;
	private final long titleIdOfRegisteredTitle = 19L;

	@Async
	@EventListener
	public void checkRegisteredTitle(RegisteredEvent registeredEvent) {
		getRegisteredTitle(registeredEvent.getMember());
	}

	public void getRegisteredTitle(Member member) {
		Optional<Title> optionalTitle = titleRepository.findById(titleIdOfRegisteredTitle);
		if (optionalTitle.isEmpty()) {
			throw new NotExistTitleException(member, titleIdOfRegisteredTitle);
		}
		if (earnedTitleRepository.existsByTitle_IdAndMember(titleIdOfRegisteredTitle, member)) {
			log.info(
				"회원가입 칭호를 받은 사용자 " + member.getNickname() + "님이 다시 회원가입을 칭호를 획득 하였습니다. 확인바랍니다.");
		}
		earnedTitleRepository.save(EarnedTitle.create(member, optionalTitle.get()));
	}
}