import type { HttpClientError } from './HttpClientError';

export interface ResponseErrorInterceptor {
	intercept(err: HttpClientError): any | Promise<any>; // TODO Improve typing (don't use "any")
}
