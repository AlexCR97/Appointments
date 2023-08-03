import { TenantApi } from '$lib/api/tenants';
import type { PageLoad } from './$types';

export const load: PageLoad = async () => {
	const tenantApi = new TenantApi();

	const tenants = await tenantApi.getAsync();

	return {
		tenants
	};
};
