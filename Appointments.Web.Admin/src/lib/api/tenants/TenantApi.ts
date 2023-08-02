import { env } from '$lib/env';
import { HttpClient } from '$lib/http';
import type { CreateTenantRequest } from './CreateTenantRequest';
import type { TenantCreatedResponse } from './TenantCreatedResponse';

export class TenantApi {
	private readonly httpClient = new HttpClient({
		baseUrl: env.api.baseUrl,
		basePath: 'tenants'
	});

	async createAsync(request: CreateTenantRequest): Promise<TenantCreatedResponse> {
		return await this.httpClient.postAsync<TenantCreatedResponse>('/', request);
	}
}
