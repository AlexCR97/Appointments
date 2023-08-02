export interface OAuthTokenResponse {
	access_token: string;
	expires_in: number;
	token_type: string;
	id_token?: string;
	refresh_token?: string;
	scope?: string;
}
