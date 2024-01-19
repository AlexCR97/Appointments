import { writable } from 'svelte/store';
import type { PageAction } from './PageAction';

export const pageActions = writable<PageAction[]>([]);

export const pageTitle = writable<string>('Page Title');

export const pageSubtitle = writable<string | undefined>();
