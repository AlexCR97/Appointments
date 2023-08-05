<script lang="ts">
	import { onDestroy, onMount } from 'svelte';
	import type { Unsubscriber } from 'svelte/store';
	import { fade } from 'svelte/transition';
	import { loader } from './LoaderStore';

	let show = false;
	let unsubscribe: Unsubscriber | undefined;

	onMount(() => {
		console.log('Loader.svelte');

		unsubscribe = loader.subscribe((_show) => {
			console.log('_show', _show);
			show = _show;
		});
	});

	onDestroy(() => {
		if (unsubscribe) {
			unsubscribe();
		}
	});
</script>

{#if show}
	<div
		class="position-absolute"
		style="background-color: rgba(0, 0, 0, .35); width: 100vw; height: 100vh; z-index: 1"
		transition:fade={{ duration: 250 }}
	>
		<div class="d-flex justify-content-center align-items-center w-100 h-100">
			<div>
				<div class="d-flex justify-content-center">
					<div
						class="spinner-border text-primary"
						style="width: 60px; height: 60px"
						role="status"
					/>
				</div>
			</div>
		</div>
	</div>
{/if}
