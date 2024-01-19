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

	get accessTokenClaims(): AccessTokenClaims {
		const claims = this.accessTokenClaimsOrDefault;

		if (claims !== null) {
			return claims;
		}

		throw new Error('Cannot decode access_token');
	}

	get accessTokenClaimsOrDefault(): AccessTokenClaims | null {
		if (this.accessToken === undefined) {
			return null;
		}

		const claims = this.decodeOrDefault<RawAccessTokenClaims>(this.accessToken);

		if (claims === null) {
			return null;
		}

		return {
			email: claims['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
			exp: claims.exp,
			iss: claims.iss,
			role: claims['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
		};
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

	get idTokenClaims(): IdTokenClaims {
		if (this.idToken === undefined) {
			throw new Error('Cannot decode undefined id_token');
		}

		const claims = this.decode<RawIdTokenClaims>(this.idToken);

		return {
			email: claims['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
			exp: claims.exp,
			firstName: claims.firstName,
			id: claims.id,
			iss: claims.iss,
			lastName: claims.lastName
		};
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

	/* #region Private */

	private key(key: string) {
		return `appointments-dotnet.${key}`;
	}

	private decode<T>(jwt: string): T {
		const token = this.decodeOrDefault<T>(jwt);

		if (token !== null) {
			return token;
		}

		throw new Error('Could not decode invalid jwt');
	}

	private decodeOrDefault<T>(jwt: string): T | null {
		const parts = jwt.split('.');

		if (parts.length !== 3) {
			return null;
		}

		const payloadBase64Url = parts[1];
		const decodedPayload = this.base64UrlDecode(payloadBase64Url);

		try {
			return JSON.parse(decodedPayload);
		} catch (err) {
			return null;
		}
	}

	private base64UrlDecode(base64Url: string): string {
		const padding = '='.repeat((4 - (base64Url.length % 4)) % 4);
		const base64 = (base64Url + padding).replace(/-/g, '+').replace(/_/g, '/');
		return atob(base64);
	}

	/* #endregion */
}

interface RawAccessTokenClaims {
	exp: number;
	'http://schemas.microsoft.com/ws/2008/06/identity/claims/role': string[];
	'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress': string;
	iss: string;
}

interface AccessTokenClaims {
	exp: number;
	role: string[];
	email: string;
	iss: string;
}

interface RawIdTokenClaims {
	exp: number;
	iss: string;
	id: string;
	'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress': string;
	firstName?: string;
	lastName?: string;
}

interface IdTokenClaims {
	exp: number;
	iss: string;
	id: string;
	email: string;
	firstName?: string;
	lastName?: string;
}
