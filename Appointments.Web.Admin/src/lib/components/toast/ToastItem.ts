import { Guid } from '$lib/guid';

export class ToastItem {
	readonly id = Guid.random();
	readonly title?: string;
	readonly content?: string;
	readonly dismissable?: boolean;
	shown = false;

	constructor(options?: { title?: string; content?: string; dismissable?: boolean }) {
		this.title = options?.title;
		this.content = options?.content;
		this.dismissable = options?.dismissable;
	}
}
