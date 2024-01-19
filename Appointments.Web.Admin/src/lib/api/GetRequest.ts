export class GetRequest {
	constructor(
		readonly pageIndex: number,
		readonly pageSize: number,
		readonly sort?: string | undefined,
		readonly filter?: string | undefined
	) {}
}
