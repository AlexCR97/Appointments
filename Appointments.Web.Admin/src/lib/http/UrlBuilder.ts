import type { QueryParams } from './QueryParams';

export class UrlBuilder {
	private readonly pathParts: string[] = [];
	private readonly query = new URLSearchParams();

	constructor(readonly baseUrl: string) {
		this.pathParts.push(baseUrl);
	}

	withPath(path: string | undefined): UrlBuilder {
		if (path === undefined || path.trim().length === 0) {
			return this;
		}

		if (path.trim() === '/') {
			return this;
		}

		this.pathParts.push(path);

		return this;
	}

	withQuery(query: QueryParams | undefined): UrlBuilder {
		if (query === undefined) {
			return this;
		}

		Object.keys(query).forEach((key) => {
			const value = query[key];
			this.query.append(key, value);
		});

		return this;
	}

	build(): string {
		const absolutePath = this.pathParts.join('/');
		const url = new URL(absolutePath);
		url.search = this.query.toString();
		return url.toString();
	}
}
