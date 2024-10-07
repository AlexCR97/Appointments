import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ButtonType } from './ButtonType';
import { Router } from '@angular/router';
import { ColorVariant } from '../ColorVariant';

@Component({
  selector: 'app-button',
  standalone: true,
  imports: [],
  templateUrl: './button.component.html',
  styleUrl: './button.component.scss',
})
export class ButtonComponent {
  @Input() label: string | undefined;
  @Input() routerLink: string | undefined;
  @Input() type: ButtonType | undefined;
  @Input() variant: ColorVariant | undefined;

  @Output() click = new EventEmitter();

  constructor(private readonly router: Router) {}

  get buttonClassName(): string {
    return `btn btn-${this.variant}`;
  }

  onClick() {
    if (this.routerLink) {
      this.router.navigateByUrl(this.routerLink);
      return;
    }

    this.click.emit();
  }
}
