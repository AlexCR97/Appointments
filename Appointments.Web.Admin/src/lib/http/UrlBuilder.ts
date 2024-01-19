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

		if (path.startsWith('/')) {
			const normalizedPath = this.removeFirstCharacter(path);
			this.pathParts.push(normalizedPath);
		} else {
			this.pathParts.push(path);
		}

		return this;
	}

	withQuery(query: QueryParams | undefined): UrlBuilder {
		if (query === undefined) {
			return this;
		}

		Object.keys(query).forEach((key) => {
			const value = query[key];

			if (value !== undefined) {
				this.query.append(key, value);
			}
		});

		return this;
	}

	build(): string {
		const absolutePath = this.pathParts.join('/');
		const url = new URL(absolutePath);
		url.search = this.query.toString();
		return url.toString();
	}

	private removeFirstCharacter(str: string): string {
		if (str.length === 0) {
			throw new Error('Input string cannot be empty.');
		}

		return str.slice(1);
	}
}
