import type { HttpClientError, ResponseErrorInterceptor } from '$lib/http';
import { error } from '@sveltejs/kit';

export class ApiResponseErrorInterceptor implements ResponseErrorInterceptor {
	intercept(err: HttpClientError) {
		if (err.status === 401) {
			return this.handleUnauthorizedError(err);
		}

		if (err.status === 403) {
			return this.handleForbiddenError(err);
		}

		if (err.status >= 400 && err.status <= 499) {
			return this.handleClientSideError(err);
		}

		if (err.status >= 500) {
			return this.handleServerSideError(err);
		}

		return err;
	}

	private handleUnauthorizedError(err: HttpClientError) {
		return error(err.status, {
			message: 'You are not authorized to view this page'
		});
	}

	private handleForbiddenError(err: HttpClientError) {
		return error(err.status, {
			message: 'You are not authorized to view this page'
		});
	}

	private handleClientSideError(err: HttpClientError) {
		console.error('client side error');
		// TODO Show error notification
		return err;
	}

	private handleServerSideError(err: HttpClientError) {
		return error(err.status, {
			message: 'You are not authorized to view this page'
		});
	}
}
