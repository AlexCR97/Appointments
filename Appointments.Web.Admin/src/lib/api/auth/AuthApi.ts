import { AuthStore } from '$lib/auth';
import { env } from '$lib/env';
import { HttpClient } from '$lib/http';
import type { LoginWithEmailRequest } from './LoginWithEmailRequest';
import type { OAuthTokenResponse } from './OAuthTokenResponse';

export class AuthApi {
	private readonly httpClient = new HttpClient({
		baseUrl: env.api.baseUrl,
		basePath: 'users' // TODO Change to auth once api has been updated
	});

	private readonly authStore = new AuthStore();

	async loginWithEmailAsync(request: LoginWithEmailRequest): Promise<void> {
		const response = await this.httpClient.postAsync<OAuthTokenResponse>('login/email', request);
		this.storeSession(response);
	}

	private storeSession(response: OAuthTokenResponse) {
		this.authStore.accessToken = response.access_token;
		this.authStore.idToken = response.id_token;
		this.authStore.refreshToken = response.refresh_token;
	}
}
