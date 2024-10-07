import { Component } from '@angular/core';
import { PageHeaderComponent } from '../../../../components/page-header/page-header.component';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ButtonComponent } from '../../../../components/button/button.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-new-tenant',
  standalone: true,
  imports: [PageHeaderComponent, ReactiveFormsModule, ButtonComponent],
  templateUrl: './new-tenant.page.html',
  styleUrl: './new-tenant.page.scss',
})
export class NewTenantPage {
  form = new FormGroup({
    name: new FormControl('', {
      validators: [Validators.required],
    }),
  });

  constructor(private readonly router: Router) {}

  onSubmit() {
    console.log('form.value', this.form.value);

    this.router.navigateByUrl('/portal/tenants');
  }
}
