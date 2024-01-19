export interface RequestInterceptor {
	intercept(request: RequestInit): RequestInit | Promise<RequestInit>;
}
