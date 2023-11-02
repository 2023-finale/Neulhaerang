package com.finale.neulhaerang.domain.routine.service;

import java.time.LocalDate;
import java.util.List;

import com.finale.neulhaerang.domain.routine.dto.request.RoutineCreateReqDto;
import com.finale.neulhaerang.domain.routine.dto.request.RoutineModifyReqDto;

public interface RoutineService {
	void createRoutine(RoutineCreateReqDto routineCreateReqDto);

	List<?> findRoutineByMemberAndDate(LocalDate date);

	void modifyDailyRoutineCheckByDailyRoutineId(Long dailyRoutineId);

	void modifyRoutineContentAndRepeatedAndAlarmAndAlarmTimeByRoutineId(RoutineModifyReqDto routineModifyReqDto);
}
