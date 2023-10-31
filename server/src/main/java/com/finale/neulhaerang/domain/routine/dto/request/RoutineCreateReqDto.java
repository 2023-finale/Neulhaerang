package com.finale.neulhaerang.domain.routine.dto.request;

import java.time.LocalTime;
import java.util.List;

import javax.validation.constraints.NotBlank;
import javax.validation.constraints.NotNull;
import javax.validation.constraints.Size;

import com.finale.neulhaerang.domain.routine.entity.StatType;

import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Getter
@NoArgsConstructor
public class RoutineCreateReqDto {
	@NotBlank
	private String content;
	@Size(min = 1)
	@NotNull
	private List<String> repeated;
	@NotNull
	private boolean alarm;
	private LocalTime alarmTime;
	@NotNull
	private StatType statType;

	@Builder
	private RoutineCreateReqDto(String content, List<String> repeated, boolean alarm, LocalTime alarmTime,
		StatType statType) {
		this.content = content;
		this.repeated = repeated;
		this.alarm = alarm;
		this.alarmTime = alarmTime;
		this.statType = statType;
	}
}