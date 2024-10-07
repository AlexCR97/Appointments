import { Component } from '@angular/core';
import { PageHeaderComponent } from '../../../../components/page-header/page-header.component';
import { MenuItem } from '../../../../components/MenuItem';

@Component({
  selector: 'app-tenant-profile',
  standalone: true,
  imports: [PageHeaderComponent],
  templateUrl: './tenant-profile.page.html',
  styleUrl: './tenant-profile.page.scss',
})
export class TenantProfilePage {
  readonly tenant = {
    id: '00000000-0000-0000-0000-000000000001',
    name: 'Global Tenant',
  };

  readonly pageHeaderMenuItems: MenuItem[] = [
    {
      id: 'back',
      label: 'Back',
      variant: 'secondary',
    },
    {
      id: 'refresh',
      label: 'Refresh',
      variant: 'secondary',
    },
  ];
}
