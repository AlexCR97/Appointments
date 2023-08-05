<script lang="ts">
	import { goto } from '$app/navigation';
	import { CreateTenantRequest, TenantApi } from '$lib/api/tenants';
	import { ToastItem, loader, toastOutlet } from '$lib/components';
	import { Input } from '$lib/components/forms';
	import { pageActions, pageTitle } from '$lib/components/page-header';

	const tenantApi = new TenantApi();

	const formId = 'createTenantForm';

	let name = '';
	let slogan = '';
	let urlId = '';

	pageTitle.set('Create Tenant');

	pageActions.set([
		{
			label: 'Back',
			back: true,
			variant: 'dark'
		},
		{
			label: 'Submit',
			form: formId,
			variant: 'primary'
		}
	]);

	async function onSubmit(e: Event) {
		e.preventDefault();

		await loader.load(async () => {
			await tenantApi.createAsync(new CreateTenantRequest(name, slogan, urlId));
		});

		toastOutlet.push(
			new ToastItem({
				title: 'Successfully created tenant',
				dismissable: true
			})
		);

		await goto('/portal/tenants');
	}
</script>

<div class="container py-4">
	<form id={formId} method="post" on:submit={onSubmit}>
		<Input class="mb-4" label="Name" placeholder="The company name" bind:value={name} />
		<Input class="mb-4" label="Slogan" placeholder="The company's slogan" bind:value={slogan} />
		<Input class="mb-4" label="URL ID" placeholder="https://mycompany.com" bind:value={urlId} />
	</form>
</div>
