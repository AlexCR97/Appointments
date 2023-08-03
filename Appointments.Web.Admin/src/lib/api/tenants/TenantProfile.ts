export interface TenantProfile {
	id: string;
	createdAt: Date;
	createdBy?: string;
	updatedAt?: Date;
	updatedBy?: string;
	deletedAt?: Date;
	deletedBy?: string;
	extensions?: object;
	name: string;
	slogan?: string;
	urlId: string;
	logo?: string;
	socialMediaContacts: any[];
	weeklySchedule?: any;
}
