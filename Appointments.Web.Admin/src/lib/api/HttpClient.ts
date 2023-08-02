import type { HttpMethod } from '@sveltejs/kit';
import type { QueryParams } from './QueryParams';
import { UrlBuilder } from './UrlBuilder';
import { HttpClientError } from './HttpClientError';

export interface HttpRequestOptions {
	body?: object;
	query?: QueryParams;
}

export interface HttpClientOptions {
	baseUrl: string;
	basePath?: string;

	// TODO Add support for interceptors
	requestInterceptor?: any;
	responseInterceptor?: any;
}

export class HttpClient {
	readonly baseUrl: string;

	constructor(options: HttpClientOptions) {
		this.baseUrl = new UrlBuilder(options.baseUrl).withPath(options.basePath).build();
	}

	async deleteAsync<TResponse>(resource: string, options?: HttpRequestOptions): Promise<TResponse> {
		return await this.sendAsync('DELETE', this.baseUrl, resource, options);
	}

	async getAsync<TResponse>(resource: string, options?: HttpRequestOptions): Promise<TResponse> {
		return await this.sendAsync('GET', this.baseUrl, resource, options);
	}

	async postAsync<TResponse>(
		resource: string,
		body: object,
		options?: HttpRequestOptions
	): Promise<TResponse> {
		options = options ?? {};
		options.body = body;
		return await this.sendAsync('POST', this.baseUrl, resource, options);
	}

	async putAsync<TResponse>(
		resource: string,
		body: object,
		options?: HttpRequestOptions
	): Promise<TResponse> {
		options = options ?? {};
		options.body = body;
		return await this.sendAsync('PUT', this.baseUrl, resource, options);
	}

	private async sendAsync<TResponse>(
		method: HttpMethod,
		baseUrl: string,
		resource: string,
		options?: HttpRequestOptions
	): Promise<TResponse> {
		const url = new UrlBuilder(baseUrl).withPath(resource).withQuery(options?.query).build();

		const response = await fetch(url, {
			method,
			headers: {
				// TODO How to set authorization header?
				Authorization:
					'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhbGVAbGl2ZS5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsiVXNlci5Pd25lciIsIlRlbmFudC5Pd25lciIsIlNlcnZpY2UuT3duZXIiLCJCcmFuY2hPZmZpY2UuT3duZXIiXSwiZXhwIjoxNjkwOTEzMTc1LCJpc3MiOiJodHRwczovL2FwaS5hcHBvaW50bWVudHMuY29tL2FwaSJ9.bHhEHSO_Wfr5TwtWiODSSBFuXWCBNEIY3yMi0BAIaU0',
				'Content-Type': 'application/json'
			},
			body: options?.body !== undefined ? JSON.stringify(options.body) : undefined
		});

		if (response.ok) {
			return await response.json();
		}

		throw new HttpClientError(
			method,
			response.url,
			response.status,
			response.statusText,
			await this.extractResponseAsync(response)
		);
	}

	private async extractResponseAsync(response: Response): Promise<unknown> {
		try {
			return await response.json();
		} catch (_) {
			return await response.text();
		}
	}
}
