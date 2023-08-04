<script lang="ts">
	import { Guid } from '$lib/guid';

	export let title: string | undefined = undefined;
	export let content: any | undefined = undefined;
	export let dismissable: boolean = true;

	const toastId = Guid.random();

	export async function show() {
		const toastElement = document.getElementById(toastId);

		if (toastElement === null) {
			throw new Error(`Null element with id ${toastId}`);
		}

		const { Toast } = await import('bootstrap');
		const toastBootstrap = Toast.getOrCreateInstance(toastElement);
		toastBootstrap.show();
	}
</script>

<div id={toastId} class="toast" role="alert">
	{#if title !== undefined && title.trim().length > 0}
		<div class="toast-header">
			<strong class="me-auto">{title}</strong>
			{#if dismissable}
				<button type="button" class="btn-close" data-bs-dismiss="toast" />
			{/if}
		</div>
	{/if}
	{#if content}
		<div class="toast-body">
			{content}
		</div>
	{/if}
</div>
