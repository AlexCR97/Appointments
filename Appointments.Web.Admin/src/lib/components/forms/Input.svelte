<script lang="ts">
	import { Guid } from '$lib/guid';
	import type { HTMLInputTypeAttribute } from 'svelte/elements';

	let customClass = '';
	export { customClass as class };
	export let label = '';
	export let placeholder = '';
	export let type: HTMLInputTypeAttribute = 'text';

	export let value = '';
	const onInput = (e: any) => (value = e.target.value);

	const inputId = Guid.random();
</script>

<div class={`mb-3 ${customClass}`}>
	{#if label}
		<label for={inputId} class="form-label">{label}</label>
	{/if}
	<input id={inputId} {type} class="form-control" {placeholder} {value} on:input={onInput} />
</div>
