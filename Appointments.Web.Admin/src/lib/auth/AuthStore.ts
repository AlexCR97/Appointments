export class AuthStore {
	/* #region Access token */

	get accessToken(): string | undefined {
		return sessionStorage.getItem(this.accessTokenKey) ?? undefined;
	}

	set accessToken(value: string | undefined) {
		if (value === undefined || value.trim().length === 0) {
			sessionStorage.removeItem(this.accessTokenKey);
		} else {
			sessionStorage.setItem(this.accessTokenKey, value);
		}
	}

	private get accessTokenKey() {
		return this.key('accessToken');
	}

	/* #endregion */

	/* #region ID token */

	get idToken(): string | undefined {
		return sessionStorage.getItem(this.idTokenKey) ?? undefined;
	}

	set idToken(value: string | undefined) {
		if (value === undefined || value.trim().length === 0) {
			sessionStorage.removeItem(this.idTokenKey);
		} else {
			sessionStorage.setItem(this.idTokenKey, value);
		}
	}

	private get idTokenKey() {
		return this.key('idToken');
	}

	/* #endregion */

	/* #region Refresh token */

	get refreshToken(): string | undefined {
		return sessionStorage.getItem(this.refreshTokenKey) ?? undefined;
	}

	set refreshToken(value: string | undefined) {
		if (value === undefined || value.trim().length === 0) {
			sessionStorage.removeItem(this.refreshTokenKey);
		} else {
			sessionStorage.setItem(this.refreshTokenKey, value);
		}
	}

	private get refreshTokenKey() {
		return this.key('refreshToken');
	}

	/* #endregion */

	private key(key: string) {
		return `appointments-dotnet.${key}`;
	}
}
