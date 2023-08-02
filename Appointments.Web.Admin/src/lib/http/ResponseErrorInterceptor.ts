import type { HttpClientError } from './HttpClientError';

export interface ResponseErrorInterceptor {
	intercept<TOutgoing>(err: HttpClientError): TOutgoing | Promise<TOutgoing>;
}
