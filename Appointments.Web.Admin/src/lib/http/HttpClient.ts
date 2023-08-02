import type { HttpMethod } from '@sveltejs/kit';
import type { QueryParams } from './QueryParams';
import { UrlBuilder } from './UrlBuilder';
import { HttpClientError } from './HttpClientError';
import type { RequestInterceptor } from './RequestInterceptor';
import type { ResponseInterceptor } from './ResponseInterceptor';
import type { ResponseErrorInterceptor } from './ResponseErrorInterceptor';

export interface HttpRequestOptions {
	body?: object;
	query?: QueryParams;
}

export interface HttpClientOptions {
	baseUrl: string;
	basePath?: string;
	requestInterceptor?: RequestInterceptor;
	responseInterceptor?: ResponseInterceptor;
	responseErrorInterceptor?: ResponseErrorInterceptor;
}

export class HttpClient {
	private readonly baseUrl: string;
	private readonly requestInterceptor?: RequestInterceptor;
	private readonly responseInterceptor?: ResponseInterceptor;
	private readonly responseErrorInterceptor?: ResponseErrorInterceptor;

	constructor(options: HttpClientOptions) {
		this.baseUrl = new UrlBuilder(options.baseUrl).withPath(options.basePath).build();
		this.requestInterceptor = options.requestInterceptor;
		this.responseInterceptor = options.responseInterceptor;
		this.responseErrorInterceptor = options.responseErrorInterceptor;
	}

	async deleteAsync<TResponse>(resource: string, options?: HttpRequestOptions): Promise<TResponse> {
		return await this.sendAsync('DELETE', resource, options);
	}

	async getAsync<TResponse>(resource: string, options?: HttpRequestOptions): Promise<TResponse> {
		return await this.sendAsync('GET', resource, options);
	}

	async postAsync<TResponse>(
		resource: string,
		body: object,
		options?: HttpRequestOptions
	): Promise<TResponse> {
		options = options ?? {};
		options.body = body;
		return await this.sendAsync('POST', resource, options);
	}

	async putAsync<TResponse>(
		resource: string,
		body: object,
		options?: HttpRequestOptions
	): Promise<TResponse> {
		options = options ?? {};
		options.body = body;
		return await this.sendAsync('PUT', resource, options);
	}

	private async sendAsync<TResponse>(
		method: HttpMethod,
		resource: string,
		options?: HttpRequestOptions
	): Promise<TResponse> {
		const url = new UrlBuilder(this.baseUrl).withPath(resource).withQuery(options?.query).build();
		const request = await this.buildRequestAsync(method, options?.body, this.requestInterceptor);
		const response = await fetch(url, request);

		if (response.ok) {
			return await this.handleSuccessfulResponseAsync(response);
		}

		const httpClientError = new HttpClientError(
			method,
			response.url,
			response.status,
			response.statusText,
			await this.extractRawResponseAsync(response)
		);

		if (this.responseErrorInterceptor === undefined) {
			throw httpClientError;
		} else {
			throw await this.responseErrorInterceptor.intercept<unknown>(httpClientError);
		}
	}

	private async buildRequestAsync(
		method: HttpMethod,
		body?: object,
		interceptor?: RequestInterceptor
	): Promise<RequestInit> {
		let requestInit: RequestInit = {
			method,
			headers: {
				'Content-Type': 'application/json'
			},
			body: body !== undefined ? JSON.stringify(body) : undefined
		};

		if (interceptor) {
			requestInit = await interceptor.intercept(requestInit);
		}

		return requestInit;
	}

	private async handleSuccessfulResponseAsync<TResponse>(response: Response): Promise<TResponse> {
		let responseContent = await response.json();

		if (this.responseInterceptor) {
			responseContent = await this.responseInterceptor.intercept<TResponse, TResponse>(
				responseContent
			);
		}

		return responseContent;
	}

	private async extractRawResponseAsync(response: Response): Promise<unknown> {
		try {
			return await response.json();
		} catch (_) {
			return await response.text();
		}
	}
}
