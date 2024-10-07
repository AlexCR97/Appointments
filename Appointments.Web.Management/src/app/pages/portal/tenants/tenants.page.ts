import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { Grid, h } from 'gridjs';
import { ServerStorageOptions } from 'gridjs/dist/src/storage/server.js';
import { TColumn, TColumnSort } from 'gridjs/dist/src/types.js';
import { PaginationConfig } from 'gridjs/dist/src/view/plugin/pagination.js';
import { SearchConfig } from 'gridjs/dist/src/view/plugin/search/search.js';
import { GenericSortConfig } from 'gridjs/dist/src/view/plugin/sort/sort.js';
import { Router } from '@angular/router';
import { PageHeaderComponent } from '../../../components/page-header/page-header.component';
import { MenuItem } from '../../../components/MenuItem';

@Component({
  selector: 'app-tenants',
  standalone: true,
  imports: [PageHeaderComponent],
  templateUrl: './tenants.page.html',
  styleUrl: './tenants.page.scss',
})
export class TenantsPage implements AfterViewInit {
  readonly pageHeaderMenuItems: MenuItem[] = [
    {
      id: 'new',
      label: 'New Tenant',
      routerLink: '/portal/tenants/new',
      type: 'button',
      variant: 'primary',
    },
  ];

  @ViewChild('gridContainer', { static: true })
  readonly gridContainer!: ElementRef<HTMLDivElement>;

  readonly gridServer: ServerStorageOptions = {
    url: 'http://localhost:5000/api/tenants',
    data: async (options) => {
      return {
        data: this.tenants,
        total: this.tenants.length,
      };
    },
  };

  readonly gridColumns: TColumn[] = [
    {
      id: 'id',
      name: 'Tenant',
      formatter: (cell, row, column) => {
        const tenantId = cell!.toString();

        const tenant = this.tenants.find((x) => x.id === tenantId)!;

        return h('div', null, [
          nameLabel({ tenant, router: this.router }),
          idLabel(tenant),
        ]);

        function nameLabel(options: { tenant: Tenant; router: Router }): any {
          const { tenant, router } = options;

          return h(
            'a',
            {
              className:
                'd-block link-primary link-offset-2 link-underline-opacity-50 link-underline-opacity-100-hover',
              role: 'button',
              onClick: () => {
                router.navigate(['portal', 'tenants', tenant.id]);
              },
            },
            tenant.name
          );
        }

        function idLabel(tenant: Tenant): any {
          return h('span', { className: 'fs-8 text-secondary' }, tenant.id);
        }
      },
    },
    {
      id: 'urlId',
      name: 'URL',
    },
  ];

  readonly gridSearch: SearchConfig = {
    server: {
      url(url: string, search: string) {
        const { origin, pathname } = new URL(url);

        return new UrlBuilder(origin)
          .withPath(pathname)
          .withQuery({ search })
          .build();
      },
    },
  };

  readonly gridSort: GenericSortConfig = {
    multiColumn: true,
    server: {
      url: (prevUrl: string, [column]: TColumnSort[]) => {
        if (column === undefined || column === null) {
          return prevUrl;
        }

        const { origin, pathname } = new URL(prevUrl);
        const sortProperty = this.gridColumns[column.index];
        const sortDirection = column.direction === 1 ? 'asc' : 'desc';
        const sort = `${sortProperty} ${sortDirection}`;

        return new UrlBuilder(origin)
          .withPath(pathname)
          .withQuery({ sort })
          .build();
      },
    },
  };

  readonly gridPagination: PaginationConfig = {
    limit: 50,
    server: {
      url(prevUrl, page, limit) {
        const url = new URL(prevUrl);
        url.searchParams.append('page', page.toString());
        url.searchParams.append('limit', limit.toString());
        return url.toString();
      },
    },
  };

  readonly grid = new Grid({
    server: this.gridServer,
    columns: this.gridColumns,
    search: this.gridSearch,
    sort: this.gridSort,
    pagination: this.gridPagination,
  });

  readonly tenants = [
    {
      id: '00000000-0000-0000-0000-000000000001',
      name: 'Global Tenant',
      urlId: 'global',
    },
    {
      id: 'ab91ff07-7bf0-453d-883b-2daf2014ed8b',
      name: 'Antu DevOps',
      urlId: 'antudevops',
    },
    {
      id: '530ead16-ce09-45bb-bdd9-8539865d7c33',
      name: 'Los Pollos Hermanos',
      urlId: 'lospolloshermanos',
    },
  ];

  constructor(private readonly router: Router) {}

  ngAfterViewInit(): void {
    this.grid.render(this.gridContainer.nativeElement);
  }
}

type Tenant = {
  id: string;
  name: string;
  urlId: string;
};

export interface QueryParams {
  [key: string]: string | undefined;
}

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
