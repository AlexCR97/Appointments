import { env } from '$lib/env';
import { HttpClient } from '$lib/http';
import { ApiRequestInterceptor } from '../ApiRequestInterceptor';
import { ApiResponseErrorInterceptor } from '../ApiResponseErrorInterceptor';
import type { CreateTenantRequest } from './CreateTenantRequest';
import type { TenantCreatedResponse } from './TenantCreatedResponse';

export class TenantApi {
	private readonly httpClient = new HttpClient({
		baseUrl: env.api.baseUrl,
		basePath: 'tenants',
		requestInterceptor: new ApiRequestInterceptor(),
		responseErrorInterceptor: new ApiResponseErrorInterceptor()
	});

	async createAsync(request: CreateTenantRequest): Promise<TenantCreatedResponse> {
		return await this.httpClient.postAsync<TenantCreatedResponse>('/', request);
	}
}
