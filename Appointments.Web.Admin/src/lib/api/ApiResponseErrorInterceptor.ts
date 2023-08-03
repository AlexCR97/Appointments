import { goto } from '$app/navigation';
import type { HttpClientError, ResponseErrorInterceptor } from '$lib/http';
import { message, problemDetails, statusCode } from '$lib/error-page';

export class ApiResponseErrorInterceptor implements ResponseErrorInterceptor {
	intercept(err: HttpClientError) {
		statusCode.set(err.status);
		message.set(err.message);
		problemDetails.set(err.response);

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
		goto('/error');
		return err;
	}

	private handleForbiddenError(err: HttpClientError) {
		goto('/error');
		return err;
	}

	private handleClientSideError(err: HttpClientError) {
		// TODO Show error notification
		console.error('TODO Show error notification');
		return err;
	}

	private handleServerSideError(err: HttpClientError) {
		goto('/error');
		return err;
	}
}
