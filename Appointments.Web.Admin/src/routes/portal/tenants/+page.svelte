<script lang="ts">
	import { pageActions, pageTitle } from '$lib/components/page-header';

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
