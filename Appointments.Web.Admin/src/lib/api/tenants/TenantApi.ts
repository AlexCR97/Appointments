import { HttpClient } from '../HttpClient';
import type { CreateTenantRequest } from './CreateTenantRequest';
import type { TenantCreatedResponse } from './TenantCreatedResponse';

export class TenantApi {
	private readonly httpClient = new HttpClient({
		baseUrl: 'http://localhost:5013/api', // TODO Set with environment variable
		basePath: 'tenants'
	});

	async createAsync(request: CreateTenantRequest): Promise<TenantCreatedResponse> {
		return await this.httpClient.postAsync<TenantCreatedResponse>('/', request);
	}
}
