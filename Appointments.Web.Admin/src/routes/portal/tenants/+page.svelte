<script lang="ts">
	import { TenantApi, type TenantProfile } from '$lib/api/tenants';
	import { Grid } from '$lib/components';
	import { pageActions, pageTitle } from '$lib/components/page-header';
	import type { ServerStorageOptions } from 'gridjs/dist/src/storage/server';
	import type { SearchConfig } from 'gridjs/dist/src/view/plugin/search/search';
	import type { PaginationConfig } from 'gridjs/dist/src/view/plugin/pagination';
	import { GetRequest } from '$lib/api';

	const tenantApi = new TenantApi();

	pageTitle.set('Tenants');

	pageActions.set([
		{
			label: 'New Tenant',
			href: '/portal/tenants/create',
			variant: 'primary'
		}
	]);

	const gridColumns: (keyof TenantProfile)[] = ['id', 'createdAt', 'createdBy', 'name', 'urlId'];

	const gridPagination: PaginationConfig = {
		limit: 5,
		server: {
			url(url: string, pageIndex: number, pageSize: number) {
				const urlObj = new URL(url);
				urlObj.searchParams.append('pageIndex', pageIndex.toString());
				urlObj.searchParams.append('pageSize', pageSize.toString());
				return urlObj.toString();
			}
		}
	};

	const gridSearch: SearchConfig = {
		server: {
			url(url: string, filter: string) {
				// TODO Build filter
				const urlObj = new URL(url);
				urlObj.searchParams.append('filter', filter);
				return urlObj.toString();
			}
		}
	};

	const gridServer: ServerStorageOptions = {
		url: tenantApi.url,
		async data(options: ServerStorageOptions) {
			const url = new URL(options.url);

			const response = await tenantApi.getAsync(
				new GetRequest(
					Number(url.searchParams.get('pageIndex') ?? '0'),
					Number(url.searchParams.get('pageSize') ?? '10'),
					url.searchParams.get('sort') ?? undefined,
					url.searchParams.get('filter') ?? undefined
				)
			);

			return {
				total: response.totalCount,
				data: response.results.map((tenant) => [
					tenant.id,
					tenant.createdAt,
					tenant.createdBy,
					tenant.name,
					tenant.urlId
				])
			};
		}
	};
</script>

<Grid
	columns={gridColumns}
	pagination={gridPagination}
	search={gridSearch}
	server={gridServer}
	sort
/>
