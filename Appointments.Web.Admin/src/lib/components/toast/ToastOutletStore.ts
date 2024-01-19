import { writable } from 'svelte/store';
import type { ToastItem } from './ToastItem';

function createToastOutletStore() {
	const { subscribe, update } = writable<ToastItem[]>([]);

	return {
		push(toast: ToastItem) {
			update((currentToasts) => {
				return [...currentToasts, toast];
			});
		},

		remove(toast: ToastItem) {
			update((currentToasts) => {
				const index = currentToasts.findIndex((x) => x.id === toast.id);

				if (index !== -1) {
					currentToasts.splice(index, 1);
				}

				return currentToasts;
			});
		},

		subscribe
	};
}

export const toastOutlet = createToastOutletStore();
