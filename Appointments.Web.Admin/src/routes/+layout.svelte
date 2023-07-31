<script lang="ts">
	import 'bootstrap/dist/css/bootstrap.min.css';
	import 'bootstrap-icons/font/bootstrap-icons.min.css';
	import { onMount } from 'svelte';

	onMount(async () => {
		await import('bootstrap'); // Import on the browser
	});
</script>

<slot />
