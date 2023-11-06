package com.finale.neulhaerang.domain

import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.finale.neulhaerang.data.CheckList
import com.finale.neulhaerang.data.api.LetterApi
import com.finale.neulhaerang.data.util.onSuccess
import kotlinx.coroutines.launch
import java.time.LocalDate
import java.time.LocalDateTime

class MainScreenViewModel : ViewModel() {
    private val _selectedDateTime = mutableStateOf(LocalDateTime.now())
    private val _routineList = mutableStateListOf<CheckList>() // TODO Routine 객체 생성 후 교체
    private val _todoList = mutableStateListOf<CheckList>() // TODO todo 객체 생성 후 교체
    private val _letterText = mutableStateOf("")

    val selectedDate: LocalDate
        get() = _selectedDateTime.value.toLocalDate()
    val routineList: List<CheckList>
        get() = _routineList
    val todoList: List<CheckList>
        get() = _todoList
    val letterText: String
        get() = _letterText.value

    fun setDateTime(input: LocalDateTime) {
        _selectedDateTime.value = input
    }

    /**
     * 체크리스트를 전부 초기화하는 함수
     * API 통신에 사용
     */
    fun initCheckList(routines: List<CheckList>, todos: List<CheckList>) {
        initRoutine(routines)
        initTodo(todos)
    }

    private fun initRoutine(routines: List<CheckList>) {
        _routineList.clear()
        _routineList.addAll(routines)
    }

    private fun initTodo(todos: List<CheckList>) {
        _todoList.clear()
        _todoList.addAll(todos)
    }

    fun addRoutine(routine: CheckList) {
        _routineList.add(routine)
    }

    fun addTodo(todo: CheckList) {
        _todoList.add(todo)
    }

    fun setLetterText(input: String) {
        _letterText.value = input
    }

    fun setDataFromDateTime() {
        // 오늘의 체크리스트
        // TODO API 연결
        initCheckList(
            listOf(
                CheckList("안녕", true),
                CheckList("물 8잔 마시기", false),
            ), listOf(
                CheckList("CS 스터디 - JAVASCRIPT", false),
                CheckList("현대오토에버 이력서 작성", false),
                CheckList("CS 스터디 - React", false),
                CheckList("CS 스터디 - Kotlin", false),
            )
        )
        // 오늘의 편지
        viewModelScope.launch {
            LetterApi.instance.getLetter(selectedDate).onSuccess {
                setLetterText(it.data!!)
//                setLetterText("안녕, 싸피야!\n" + "\n" + "오늘은 내가 한 일들을 알려줄게. 아침에는 8시에 일어났어. 새로운 하루가 시작되는 건 항상 기대돼.\n" + "\n" + "오전에는 SSAFY 수업을 들었어. 오늘은 좀 어려운 프로그래밍 과제를 풀어봤어. 물론 동료들과 함께 협력하면서 문제를 해결했지. \uD83D\uDCAA\n" + "\n" + "점심 때는 친구들을 만나서 곱창전골을 먹었어. 맛있게 먹고 좋은 대화를 나누는 건 항상 즐거운 시간이야.\n" + "\n" + "양치하고 머리 감기도 잊지 않았어. 건강하게 지내려고 노력하고 있어. \uD83E\uDDB7 \uD83D\uDC87\n" + "\n" + "마지막으로, 1시에 잠자리에 들었어. 하루 동안 열심히 노력했지만, 늦게 자는 건 힘들어. \uD83D\uDE34\n" + "\n" + "앞으로도 같이 좋은 순간을 만들고 함께 성장해보자. 싸피야, 내일은 더 좋은 날이 오기를 기대해! \uD83D\uDE0A\uD83C\uDF1F")
            }
        }
    }
}