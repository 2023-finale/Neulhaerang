package com.finale.neulhaerang.global.config;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.config.http.SessionCreationPolicy;
import org.springframework.security.web.SecurityFilterChain;
import org.springframework.web.cors.CorsConfiguration;
import org.springframework.web.cors.CorsConfigurationSource;
import org.springframework.web.cors.UrlBasedCorsConfigurationSource;

import com.fasterxml.jackson.databind.ObjectMapper;

import lombok.RequiredArgsConstructor;

@Configuration
@EnableWebSecurity
@RequiredArgsConstructor
public class SecurityConfig {

	private final ObjectMapper objectMapper;
	// private final JwtTokenProvider jwtTokenProvider;
	// private final UserRedisService userRedisService;

	@Bean
	public SecurityFilterChain filterChain(HttpSecurity http) throws Exception {

		http
			.cors().configurationSource(corsConfigurationSource())
			.and()
			.httpBasic().disable()
			.csrf().disable()
			.sessionManagement().sessionCreationPolicy(SessionCreationPolicy.STATELESS)

			.and()
			//TODO: 허용할 주소 추가
			.authorizeRequests()
			//                .regexMatchers("/api/user/profile/\\d+").authenticated()
			//                .regexMatchers(HttpMethod.DELETE, "/api/user/\\d+").authenticated()
			//                .antMatchers(HttpMethod.GET, "/api/user/auth/nickname").permitAll()
			//                .antMatchers(HttpMethod.POST, "/api/user/profile").permitAll()
			.anyRequest().permitAll();
			//                .anyRequest().authenticated()

			// .and()
			// .exceptionHandling()
			// .accessDeniedHandler(new JwtAccessDeniedHandler(objectMapper))
			// .authenticationEntryPoint(new JwtAuthenticationEntryPoint(objectMapper))
			// .and()
			// .addFilterBefore(
			// 	new JwtAuthenticationFilter(jwtTokenProvider, objectMapper, userRedisService),
			// 	UsernamePasswordAuthenticationFilter.class);

		return http.build();
	}

	@Bean
	public CorsConfigurationSource corsConfigurationSource() {
		CorsConfiguration configuration = new CorsConfiguration();

		configuration.addAllowedOriginPattern("*");
		configuration.addAllowedHeader("*");
		configuration.addAllowedMethod("*");
		configuration.setAllowCredentials(true);

		UrlBasedCorsConfigurationSource source = new UrlBasedCorsConfigurationSource();

		source.registerCorsConfiguration("/**", configuration);

		return source;
	}

}