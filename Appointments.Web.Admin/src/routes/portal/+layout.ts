import { AuthStore } from '$lib/auth';
import { error } from '@sveltejs/kit';
import type { LayoutLoad } from './$types';

export const ssr = false;

export const load: LayoutLoad = () => {
	const authStore = new AuthStore();

	if (authStore.accessTokenClaimsOrDefault === null) {
		throw error(401, {
			message: 'You are not authorized to view this page'
		});
	}

	return {};
};
