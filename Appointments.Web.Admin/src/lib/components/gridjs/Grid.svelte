<script lang="ts">
	import { Guid } from '$lib/guid';
	import { Grid } from 'gridjs';
	import type { ServerStorageOptions } from 'gridjs/dist/src/storage/server';
	import type { OneDArray, TColumn, TData } from 'gridjs/dist/src/types';
	import type { PaginationConfig } from 'gridjs/dist/src/view/plugin/pagination';
	import type { SearchConfig } from 'gridjs/dist/src/view/plugin/search/search';
	import type { GenericSortConfig } from 'gridjs/dist/src/view/plugin/sort/sort';
	import { onMount } from 'svelte';

	export let columns: OneDArray<TColumn | string> | undefined = undefined;
	export let data: TData | (() => TData) | (() => Promise<TData>) | undefined = undefined;
	export let search: boolean | SearchConfig | undefined = undefined;
	export let server: ServerStorageOptions | undefined = undefined;
	export let sort: GenericSortConfig | boolean | undefined = undefined;
	export let pagination: PaginationConfig | boolean | undefined = undefined;

	const elementId = Guid.random();

	onMount(() => {
		const element = document.getElementById(elementId);

		if (element === null) {
			throw new Error('Cannot render null element');
		}

		const grid = new Grid({
			columns,
			data,
			pagination,
			search,
			server,
			sort
		});

		grid.render(element);
	});
</script>

<div id={elementId} />
