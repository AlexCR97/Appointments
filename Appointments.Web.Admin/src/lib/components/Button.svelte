<script lang="ts">
	let customClass = '';
	export { customClass as class };
	export let label = '';
	export let form: string | undefined = undefined;
	export let type: 'submit' | 'reset' | 'button' | undefined = undefined;
	export let variant:
		| 'primary'
		| 'secondary'
		| 'success'
		| 'danger'
		| 'warning'
		| 'info'
		| 'light'
		| 'dark'
		| 'link'
		| undefined = undefined;
</script>

<button class={`btn btn-${variant} ${customClass}`} {form} {type} on:click> {label} </button>
