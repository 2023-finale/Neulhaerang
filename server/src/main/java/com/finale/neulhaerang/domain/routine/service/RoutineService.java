package com.finale.neulhaerang.domain.routine.service;

import com.finale.neulhaerang.domain.member.entity.Member;
import com.finale.neulhaerang.domain.routine.dto.request.RoutineCreateReqDto;

public interface RoutineService {
	void createRoutine(Member member, RoutineCreateReqDto routineCreateReqDto);
}