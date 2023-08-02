export interface ResponseInterceptor {
	intercept<TIncoming, TOutgoing>(res: TIncoming): TOutgoing | Promise<TOutgoing>;
}
