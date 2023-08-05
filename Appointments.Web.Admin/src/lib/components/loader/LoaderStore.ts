import { writable } from 'svelte/store';

function createLoaderStore() {
	const { set, subscribe } = writable<boolean>(false);

	return {
		subscribe,
		show() {
			set(true);
		},
		hide() {
			set(false);
		},
		async load<T>(func: () => Promise<T>): Promise<T> {
			this.show();

			try {
				const result = await func();
				this.hide();
				return result;
			} catch (err) {
				this.hide();
				throw err;
			}
		}
	};
}

export const loader = createLoaderStore();
