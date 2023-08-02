export interface RequestInterceptor {
	intercept<TOutgoing>(request: RequestInit): TOutgoing | Promise<TOutgoing>;
}
