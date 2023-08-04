import { env } from '$lib/env';
import { HttpClient } from '$lib/http';
import { ApiRequestInterceptor } from '../ApiRequestInterceptor';
import { ApiResponseErrorInterceptor } from '../ApiResponseErrorInterceptor';
import type { GetRequest } from '../GetRequest';
import type { PagedResult } from '../PagedResult';
import type { CreateTenantRequest } from './CreateTenantRequest';
import type { TenantCreatedResponse } from './TenantCreatedResponse';
import type { TenantProfile } from './TenantProfile';

export class TenantApi {
	private readonly httpClient = new HttpClient({
		baseUrl: env.api.baseUrl,
		basePath: 'tenants',
		requestInterceptor: new ApiRequestInterceptor(),
		responseErrorInterceptor: new ApiResponseErrorInterceptor()
	});

	get url(): string {
		return this.httpClient.baseUrl;
	}

	async createAsync(request: CreateTenantRequest): Promise<TenantCreatedResponse> {
		return await this.httpClient.postAsync<TenantCreatedResponse>('/', request);
	}

	async deleteAsync(id: string): Promise<void> {
		await this.httpClient.deleteAsync(id);
	}

	async getAsync(request: GetRequest): Promise<PagedResult<TenantProfile>> {
		return await this.httpClient.getAsync<PagedResult<TenantProfile>>('/', {
			query: {
				pageIndex: request.pageIndex.toString(),
				pageSize: request.pageSize.toString(),
				sort: request.sort ?? '',
				filter: request.filter ?? ''
			}
		});
	}
}
