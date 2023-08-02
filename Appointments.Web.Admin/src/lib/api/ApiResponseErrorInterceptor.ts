import type { HttpClientError, ResponseErrorInterceptor } from '$lib/http';

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
		console.error('unauthorized error');
		// TODO Redirect to unauthorized page
		return err;
	}

	private handleForbiddenError(err: HttpClientError) {
		console.error('forbidden error');
		// TODO Redirect to forbidden page
		return err;
	}

	private handleClientSideError(err: HttpClientError) {
		console.error('client side error');
		// TODO Show error notification
		return err;
	}

	private handleServerSideError(err: HttpClientError) {
		console.error('server side error');
		// TODO Redirect to error page
		return err;
	}
}
