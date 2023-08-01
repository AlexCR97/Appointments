<script lang="ts">
	import { CreateTenantRequest, TenantApi } from '$lib/api/tenants';
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
		console.log('name:', name);
		console.log('slogan', slogan);
		console.log('urlId', urlId);

		const response = await tenantApi.createAsync(new CreateTenantRequest(name, slogan, urlId));
		console.log('response', response);
	}
</script>

<form id={formId} method="post" on:submit={onSubmit}>
	<Input class="mb-4" label="Name" placeholder="The company name" bind:value={name} />
	<Input class="mb-4" label="Slogan" placeholder="The company's slogan" bind:value={slogan} />
	<Input class="mb-4" label="URL ID" placeholder="https://mycompany.com" bind:value={urlId} />
</form>
