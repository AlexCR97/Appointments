<script lang="ts">
	import { TenantApi, type TenantProfile } from '$lib/api/tenants';
	import { Grid } from '$lib/components';
	import { pageActions, pageTitle } from '$lib/components/page-header';
	import type { ServerStorageOptions } from 'gridjs/dist/src/storage/server';
	import type { SearchConfig } from 'gridjs/dist/src/view/plugin/search/search';
	import type { PaginationConfig } from 'gridjs/dist/src/view/plugin/pagination';
	import { GetRequest } from '$lib/api';
	import type { GenericSortConfig } from 'gridjs/dist/src/view/plugin/sort/sort';
	import type { TColumnSort } from 'gridjs/dist/src/types';
	import { UrlBuilder } from '$lib/http';

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
			url(url: string, search: string) {
				const { origin, pathname } = new URL(url);

				// TODO Create filter builder
				const filter = `name.toLower().contains("${search}") or urlId.toLower().contains("${search}")`;

				return new UrlBuilder(origin).withPath(pathname).withQuery({ filter }).build();
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

	const gridSort: GenericSortConfig = {
		multiColumn: false,
		server: {
			url(url: string, [column]: TColumnSort[]) {
				if (column === undefined || column === null) {
					return url;
				}

				const { origin, pathname } = new URL(url);
				const sortProperty = gridColumns[column.index];
				const sortDirection = column.direction === 1 ? 'asc' : 'desc';

				return new UrlBuilder(origin)
					.withPath(pathname)
					.withQuery({
						sort: `${sortProperty} ${sortDirection}`
					})
					.build();
			}
		}
	};
</script>

<Grid
	columns={gridColumns}
	pagination={gridPagination}
	search={gridSearch}
	server={gridServer}
	sort={gridSort}
/>
