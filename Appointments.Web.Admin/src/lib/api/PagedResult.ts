export interface PagedResult<T> {
	pageIndex: number;
	pageSize: number;
	totalCount: number;
	results: T[];
}
