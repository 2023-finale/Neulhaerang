package com.finale.neulhaerang.global.event.handler;

import static org.assertj.core.api.Assertions.*;

import java.util.List;

import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;

import com.finale.neulhaerang.domain.title.entity.EarnedTitle;
import com.finale.neulhaerang.domain.title.entity.Title;
import com.finale.neulhaerang.domain.title.repository.EarnedTitleRepository;
import com.finale.neulhaerang.domain.title.repository.TitleRepository;
import com.finale.neulhaerang.global.exception.title.NotExistTitleException;
import com.finale.neulhaerang.global.util.BaseTest;

class RegisteredTitleHandlerTest extends BaseTest {
	@Autowired
	private TitleRepository titleRepository;
	@Autowired
	private RegisteredTitleHandler registeredTitleHandler;
	@Autowired
	private EarnedTitleRepository earnedTitleRepository;

	@DisplayName("회원가입을 하면 회원가입 칭호가 발급됩니다.")
	@Test
	void When_RegisterMember_Expect_GetRegisterTitle() {
		// given
		Title title = createTitle(19L, "슬늘생", "슬늘생");
		titleRepository.save(title);

		// when
		registeredTitleHandler.getRegisteredTitle(member);
		// then
		List<EarnedTitle> earnedTitles = earnedTitleRepository.findAll();
		assertThat(earnedTitles).hasSize(1);
	}

	@DisplayName("회원가입 시, 칭호가 존재하지 않는다면 에러가 납니다.")
	@Test
	void When_RegisterMemberWithNothingTitle_Expect_NotExistTitleException() {
		// given // when // then
		assertThatThrownBy(() -> registeredTitleHandler.getRegisteredTitle(member))
			.isInstanceOf(NotExistTitleException.class);
	}

	private Title createTitle(Long id, String name, String content) {
		return Title.builder()
			.id(id)
			.name(name)
			.content(content)
			.build();
	}
}