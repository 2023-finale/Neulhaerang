package com.finale.neulhaerang.domain.member.controller;

import java.util.List;

import javax.validation.Valid;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PatchMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.finale.neulhaerang.domain.member.dto.request.CharacterModifyReqDto;
import com.finale.neulhaerang.domain.member.dto.request.StatRecordReqDto;
import com.finale.neulhaerang.domain.member.dto.response.MemberCharacterResDto;
import com.finale.neulhaerang.domain.member.dto.response.MemberProfileResDto;
import com.finale.neulhaerang.domain.member.dto.response.MemberStatusResDto;
import com.finale.neulhaerang.domain.member.dto.response.StatListResDto;
import com.finale.neulhaerang.domain.member.dto.response.StatRecordListResDto;
import com.finale.neulhaerang.domain.member.service.MemberService;

import io.swagger.annotations.Api;
import io.swagger.annotations.ApiOperation;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;

@Api(value = "유저 API", tags = {"Member"})
@RestController
@RequiredArgsConstructor
@RequestMapping("/member")
@Slf4j
public class MemberController {
	private final MemberService memberService;

	@ApiOperation(value = "멤버 상태 조회", notes = "멤버 상태 정보 조회")
	@GetMapping("/status/{memberId}")
	public ResponseEntity<MemberStatusResDto> findStatusByMemberId(@PathVariable long memberId) {
		return ResponseEntity.status(HttpStatus.OK).body(memberService.findStatusByMemberId(memberId));
	}

	@ApiOperation(value = "멤버 캐릭터 조회", notes = "멤버 캐릭터 정보 조회")
	@GetMapping("/character/{memberId}")
	public ResponseEntity<MemberCharacterResDto> findCharacterByMemberId(@PathVariable long memberId) {
		return ResponseEntity.status(HttpStatus.OK).body(memberService.findCharacterByMemberId(memberId));
	}

	@ApiOperation(value = "멤버 캐릭터 정보 변경", notes = "멤버 캐릭터 정보 변경")
	@PatchMapping("/character")
	public ResponseEntity<Void> modifyCharacterByMemberId(
		@RequestBody @Valid CharacterModifyReqDto characterModifyReqDto) {
		memberService.modifyCharacterInfoByMember(characterModifyReqDto);
		return ResponseEntity.status(HttpStatus.OK).build();
	}

	@ApiOperation(value = "회원 탈퇴", notes = "로그인한 회원 탈퇴")
	@PatchMapping("/withdrawl")
	public ResponseEntity<Void> removeMember() {
		memberService.removeMember();
		return ResponseEntity.status(HttpStatus.OK).build();
	}

	@ApiOperation(value = "멤버 스탯 전체 조회", notes = "멤버 전체 스탯을 조회")
	@GetMapping("/stat/{memberId}")
	public ResponseEntity<List<StatListResDto>> findAllStatsByMemberId(@PathVariable long memberId) {
		return ResponseEntity.status(HttpStatus.OK).body(memberService.findAllStatsByMemberId(memberId));
	}

	@ApiOperation(value = "멤버 특정 스탯 동향 조회", notes = "멤버 특정 스탯 동향 조회")
	@GetMapping("/stat")
	public ResponseEntity<int[]> findStatChangeRecordLastDaysByStatType(@RequestParam int statType) {
		return ResponseEntity.status(HttpStatus.OK)
			.body(memberService.findStatChangeRecordLastDaysByStatType(statType));
	}

	@ApiOperation(value = "멤버 스탯 변경 내역", notes = "멤버 특정 스탯 변경 내역 리스트 조회")
	@GetMapping("/stat/detail")
	public ResponseEntity<List<StatRecordListResDto>> findStatChangeRecordByStatType(@RequestParam int statType,
		@RequestParam int page) {
		return ResponseEntity.status(HttpStatus.OK).body(memberService.findStatChangeRecordByStatType(statType, page));
	}

	@ApiOperation(value = "멤버 프로필 조회", notes = "멤버 레벨 및 경험치, 호칭, 닉네임 조회")
	@GetMapping("/{memberId}")
	public ResponseEntity<MemberProfileResDto> findMemberProfileByMemberId(@PathVariable long memberId) {
		return ResponseEntity.status(HttpStatus.OK).body(memberService.findMemberProfileByMemberId(memberId));
	}

	@ApiOperation(value = "피로도 저장", notes = "받아온 피로도를 저장")
	@PostMapping("/stat")
	public ResponseEntity<Void> recordTiredness(@RequestParam int tiredness) {
		memberService.recordTiredness(tiredness);
		return ResponseEntity.status(HttpStatus.OK).build();
	}

	@ApiOperation(value = "스탯 개발자용 저장", notes = "")
	@PostMapping("/stat/example")
	public ResponseEntity<Void> createStat(@RequestBody StatRecordReqDto statRecordReqDto) {
		memberService.createStat(statRecordReqDto);
		return ResponseEntity.status(HttpStatus.OK).build();
	}

}
