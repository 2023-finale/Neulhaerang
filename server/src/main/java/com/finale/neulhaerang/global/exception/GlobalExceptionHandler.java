package com.finale.neulhaerang.global.exception;

import java.net.BindException;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.HttpRequestMethodNotSupportedException;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;
import org.springframework.web.method.annotation.MethodArgumentTypeMismatchException;

import com.finale.neulhaerang.global.exception.characterInfo.NonExistCharacterInfoException;

import lombok.extern.slf4j.Slf4j;

@Slf4j
@RestControllerAdvice
public class GlobalExceptionHandler {

	@ExceptionHandler(NonExistCharacterInfoException.class)
	protected ResponseEntity<ErrorResponse> nonExistCharacterInfoException(NonExistCharacterInfoException e) {
		log.error("jwt error", e);
		ErrorResponse errorResponse = ErrorResponse.of(ErrorCode.NON_VALID_TOKEN.getErrorCode(), ErrorCode.NON_VALID_TOKEN.getMessage());
		return ResponseEntity.status(ErrorCode.NON_VALID_TOKEN.getHttpStatus())
			.body(errorResponse);
	}

	// @ExceptionHandler(CAuthenticationEntryPointException.class)
	// @ResponseStatus(HttpStatus.BAD_REQUEST)
	// protected ResponseEntity<ErrorResponse> authenticationEntryPointException(CAuthenticationEntryPointException e) {
	// 	log.error("authentication exception");
	// 	ErrorResponse errorResponse = ErrorResponse.of(HttpStatus.BAD_REQUEST.toString(), e.getMessage());
	// 	return ResponseEntity.status(HttpStatus.UNAUTHORIZED)
	// 		.body(errorResponse);
	// }

	@ExceptionHandler(BindException.class)
	protected ResponseEntity<ErrorResponse> handleBindException(BindException e) {
		log.error("handleBindException", e);
		ErrorResponse errorResponse = ErrorResponse.of(HttpStatus.BAD_REQUEST.toString(), e.getMessage());
		return ResponseEntity.status(HttpStatus.BAD_REQUEST)
			.body(errorResponse);
	}

	@ExceptionHandler(MethodArgumentTypeMismatchException.class)
	protected ResponseEntity<ErrorResponse> handleMethodArgumentTypeMismatchException(
		MethodArgumentTypeMismatchException e) {
		log.error("handleMethodArgumentTypeMismatchException", e);
		ErrorResponse errorResponse = ErrorResponse.of(HttpStatus.BAD_REQUEST.toString(), e.getMessage());
		return ResponseEntity.status(HttpStatus.BAD_REQUEST)
			.body(errorResponse);
	}

	@ExceptionHandler(HttpRequestMethodNotSupportedException.class)
	protected ResponseEntity<ErrorResponse> handleHttpRequestMethodNotSupportedException(
		HttpRequestMethodNotSupportedException e) {
		log.error("handleHttpRequestMethodNotSupportedException", e);
		ErrorResponse errorResponse = ErrorResponse.of(HttpStatus.METHOD_NOT_ALLOWED.toString(), e.getMessage());
		return ResponseEntity.status(HttpStatus.METHOD_NOT_ALLOWED).body(errorResponse);
	}

	@ExceptionHandler(Exception.class)
	protected ResponseEntity<ErrorResponse> handlerException(Exception e) {
		log.error("Exception", e);
		ErrorResponse errorResponse = ErrorResponse.of(HttpStatus.INTERNAL_SERVER_ERROR.toString(), e.getMessage());
		return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(errorResponse);
	}
}