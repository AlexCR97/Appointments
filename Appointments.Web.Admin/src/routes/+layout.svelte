<script lang="ts">
	import 'bootstrap/dist/css/bootstrap.min.css';
	import 'bootstrap-icons/font/bootstrap-icons.min.css';
	import 'gridjs/dist/theme/mermaid.min.css';
	import { onMount } from 'svelte';
	import { ToastOutlet } from '$lib/components';

	onMount(async () => {
		// Import frontend dependencies in the client side
		await import('bootstrap');
		await import('gridjs');
	});
</script>

<ToastOutlet />

<slot />
