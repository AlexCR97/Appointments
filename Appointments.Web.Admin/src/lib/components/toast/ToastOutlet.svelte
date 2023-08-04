<script lang="ts">
	import { tick } from 'svelte';
	import Toast from './Toast.svelte';
	import { toastOutlet } from './ToastOutletStore';

	let toastContainer: HTMLElement;

	toastOutlet.subscribe((toasts) => {
		tick().then(async () => {
			for (const toast of toasts) {
				const toastComponent = new Toast({
					target: toastContainer,
					props: {
						title: toast.title,
						content: toast.content,
						dismissable: true
					}
				});

				await toastComponent.show();
				toastOutlet.remove(toast);
			}
		});
	});
</script>

<Toast />

<div bind:this={toastContainer} class="toast-container position-fixed top-0 end-0 p-3" />
