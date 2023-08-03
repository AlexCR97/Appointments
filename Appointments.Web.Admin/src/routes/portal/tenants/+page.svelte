<script lang="ts">
	import { pageActions, pageTitle } from '$lib/components/page-header';
	import type { PageData } from './$types';

	export let data: PageData;

	pageTitle.set('Tenants');

	pageActions.set([
		{
			label: 'New Tenant',
			href: '/portal/tenants/create',
			variant: 'primary'
		}
	]);
</script>

<p class="text-center">TODO: Display Tenants</p>

{#each data.tenants.results as tenant}
	<p>
		<code>
			{JSON.stringify(tenant)}
		</code>
	</p>
{/each}
