<script lang="ts">
	import { onMount } from 'svelte';

	onMount(async () => {
		const { Popover } = await import('bootstrap');
		const popoverTriggerList = document.querySelectorAll('[data-bs-toggle="popover"]');
		[...popoverTriggerList].map((popoverTriggerEl) => new Popover(popoverTriggerEl));
	});
</script>

<h1>Welcome to SvelteKit</h1>
<p>Visit <a href="https://kit.svelte.dev">kit.svelte.dev</a> to read the documentation</p>

<button
	type="button"
	class="btn btn-lg btn-danger"
	data-bs-toggle="popover"
	data-bs-title="Popover title"
	data-bs-content="And here's some amazing content. It's very engaging. Right?"
>
	Click to toggle popover
</button>

<i class="bi bi-airplane-engines-fill" />
