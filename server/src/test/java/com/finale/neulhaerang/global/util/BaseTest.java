package com.finale.neulhaerang.global.util;

import javax.transaction.Transactional;

import org.junit.Ignore;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.TestInstance;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.ActiveProfiles;

import com.finale.neulhaerang.domain.member.entity.CharacterInfo;
import com.finale.neulhaerang.domain.member.entity.Member;
import com.finale.neulhaerang.domain.member.repository.CharacterInfoRepository;
import com.finale.neulhaerang.domain.member.repository.MemberRepository;

@SpringBootTest
@ActiveProfiles("test")
@TestInstance(TestInstance.Lifecycle.PER_CLASS)
@Transactional
@Ignore
public class BaseTest {
	@Autowired
	protected MemberRepository memberRepository;
	@Autowired
	protected CharacterInfoRepository characterInfoRepository;

	protected Member member;
	protected CharacterInfo characterInfo;

	@BeforeAll
	public void createTestMember() {
		Member member = Member.builder()
			.nickname("늘해랑")
			.kakaoId(1234L)
			.build();
		this.member = memberRepository.save(member);
		this.characterInfo = characterInfoRepository.save(CharacterInfo.create(this.member));
	}
}
