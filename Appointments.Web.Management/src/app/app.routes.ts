import { Routes } from '@angular/router';
import { LoginPage } from './pages/login/login.page';
import { TenantsPage } from './pages/portal/tenants/tenants.page';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'login',
  },
  {
    path: 'login',
    component: LoginPage,
  },
  {
    path: 'portal',
    loadComponent: () =>
      import('./pages/portal/portal.page').then((x) => x.PortalPage),
    children: [
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'tenants',
      },
      {
        path: 'tenants',
        component: TenantsPage,
      },
      {
        path: 'tenants/new',
        loadComponent: () =>
          import('./pages/portal/tenants/new-tenant/new-tenant.page').then(
            (x) => x.NewTenantPage
          ),
      },
      {
        path: 'tenants/:tenantId',
        loadComponent: () =>
          import(
            './pages/portal/tenants/tenant-profile/tenant-profile.page'
          ).then((x) => x.TenantProfilePage),
      },
      {
        path: 'users',
        loadComponent: () =>
          import('./pages/portal/users/users.page').then((x) => x.UsersPage),
      },
    ],
  },
];
