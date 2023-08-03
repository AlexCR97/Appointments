import { writable } from 'svelte/store';

export const statusCode = writable<number | undefined>();
export const message = writable<string | undefined>();
export const problemDetails = writable<any | undefined>(); // TODO Set ProblemDetails type
