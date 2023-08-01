import type { ButtonType, Variant } from '$lib/types';

export interface MenuItem {
	back?: boolean;
	icon?: string;
	form?: string;
	href?: string;
	label?: string;
	type?: ButtonType;
	variant?: Variant;
	click?: () => void;
}
