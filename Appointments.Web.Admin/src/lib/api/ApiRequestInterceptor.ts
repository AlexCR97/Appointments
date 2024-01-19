import { AuthStore } from '$lib/auth';
import type { RequestInterceptor } from '$lib/http';

export class ApiRequestInterceptor implements RequestInterceptor {
	private readonly authStore = new AuthStore();

	intercept(request: RequestInit): RequestInit | Promise<RequestInit> {
		if (this.authStore.accessToken !== undefined) {
			request = this.withHeader(request, 'Authorization', `Bearer ${this.authStore.accessToken}`);
		}

		return request;
	}

	private withHeader(request: RequestInit, key: string, value: string): RequestInit {
		request.headers = request.headers ?? {};
		(request.headers as { [key: string]: string })[key] = value;
		return request;
	}
}
