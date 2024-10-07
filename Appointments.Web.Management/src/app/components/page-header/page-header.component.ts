import { Component, Input } from '@angular/core';
import { MenuItem } from '../MenuItem';
import { ButtonComponent } from '../button/button.component';

@Component({
  selector: 'app-page-header',
  standalone: true,
  imports: [ButtonComponent],
  templateUrl: './page-header.component.html',
  styleUrl: './page-header.component.scss',
})
export class PageHeaderComponent {
  @Input() title: string = 'Page Title';
  @Input() subtitle: string | undefined;
  @Input() menuItems: MenuItem[] = [];

  getRouterLink(item: MenuItem): string | undefined {
    if (item.routerLink === undefined || item.routerLink === null) {
      return undefined;
    }

    if (typeof item.routerLink === 'string') {
      return item.routerLink;
    }

    return item.routerLink();
  }

  onButtonClick(menuItem: MenuItem) {
    if (menuItem.click) {
      menuItem.click();
    }
  }
}
