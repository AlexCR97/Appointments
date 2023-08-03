<script lang="ts">
	import { onMount } from 'svelte';
	import { message, problemDetails, statusCode } from './ErrorPageStore';

	onMount(() => {
		// console.log('$statusCode', $statusCode);
		// console.log('$message', $message);
		// console.log('$problemDetails', $problemDetails);
	});
</script>

<h1 class="text-center">{$statusCode ?? 500}</h1>
<h2 class="text-center">{$message ?? 'An unexpected error occurred'}</h2>

{#if $problemDetails !== undefined && $problemDetails !== null}
	<code>
		<pre>
{JSON.stringify($problemDetails, undefined, 2)}
  </pre>
	</code>
{/if}
