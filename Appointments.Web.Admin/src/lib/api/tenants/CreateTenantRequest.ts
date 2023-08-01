export class CreateTenantRequest {
	constructor(readonly name: string, readonly slogan?: string, readonly urlIr?: string) {}
}
