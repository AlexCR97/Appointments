<script lang="ts">
	import { TenantApi, type TenantProfile } from '$lib/api/tenants';
	import { Button, Grid, loader } from '$lib/components';
	import { pageActions, pageTitle } from '$lib/components/page-header';
	import type { ServerStorageOptions } from 'gridjs/dist/src/storage/server';
	import type { SearchConfig } from 'gridjs/dist/src/view/plugin/search/search';
	import type { PaginationConfig } from 'gridjs/dist/src/view/plugin/pagination';
	import { GetRequest } from '$lib/api';
	import type { GenericSortConfig } from 'gridjs/dist/src/view/plugin/sort/sort';
	import type { TColumn, TColumnSort } from 'gridjs/dist/src/types';
	import { UrlBuilder } from '$lib/http';
	import { h } from 'gridjs';
	import { tick } from 'svelte';
	import { Popover } from 'bootstrap';
	import { goto } from '$app/navigation';
	import { wait } from '$lib/time';

	const tenantApi = new TenantApi();

	pageTitle.set('Tenants');

	pageActions.set([
		{
			label: 'New Tenant',
			href: '/portal/tenants/create',
			variant: 'primary'
		}
	]);

	const gridColumns: (keyof TenantProfile | TColumn)[] = [
		'id',
		'createdAt',
		'createdBy',
		'name',
		'urlId',
		{
			name: 'Actions',
			formatter(cell, row, column) {
				const viewButtonComponent = h(
					'button',
					{
						className: 'btn btn-primary',
						async onClick() {
							const idCell = row.cell(0);
							const id = idCell.data?.toString();
							await goto(`/portal/tenants/${id}`);
						}
					},
					'View'
				);

				const popoverToggleId = `popoverToggle__${row.id}`;
				let popover: Popover | undefined;

				const popoverToggleComponent = h(
					'button',
					{
						id: popoverToggleId,
						className: 'btn btn-primary',
						'data-bs-toggle': 'popover',
						onClick() {
							if (popover !== undefined) {
								return;
							}

							const popoverTriggerElement = document.getElementById(popoverToggleId);

							if (popoverTriggerElement === null) {
								throw new Error(`Null element ${popoverToggleId}`);
							}

							tick().then(() => {
								popover = new Popover(popoverTriggerElement, {
									html: true,
									trigger: 'focus',
									content() {
										const btnWrapper = document.createElement('div');

										const option1 = new Button({
											target: btnWrapper,
											props: {
												class: 'w-100',
												label: 'Delete',
												variant: 'danger'
											}
										});

										option1.$on('click', async () => {
											const id = row.cell(0).data?.toString();

											if (id === undefined) {
												throw new Error('Null id');
											}

											await tenantApi.deleteAsync(id);
										});

										return btnWrapper;
									}
								});

								popover.show();
							});
						}
					},
					h('i', { className: 'bi bi-three-dots-vertical' })
				);

				const buttonGroup = h('div', { className: 'btn-group ' }, [
					viewButtonComponent,
					popoverToggleComponent
				]);

				return h(
					'div',
					{ className: 'btn-group d-flex justify-content-center align-items-center' },
					buttonGroup
				);
			}
		}
	];

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
			return await loader.load(async () => {
				const { searchParams } = new URL(options.url);

				const response = await tenantApi.getAsync(
					new GetRequest(
						Number(searchParams.get('pageIndex') ?? '0'),
						Number(searchParams.get('pageSize') ?? '10'),
						searchParams.get('sort') ?? undefined,
						searchParams.get('filter') ?? undefined
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
			});
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

<div class="p-4">
	<Grid
    server={gridServer}
    columns={gridColumns}
    search={gridSearch}
    sort={gridSort}
		pagination={gridPagination}
	/>
</div>
