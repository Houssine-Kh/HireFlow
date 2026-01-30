import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

// PrimeNG Imports
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { MessageModule } from 'primeng/message';
import { RippleModule } from 'primeng/ripple'; 

// Our Store
import { AuthStore } from '../../../infrastructure/auth/auth.store';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    InputTextModule,
    PasswordModule,
    ButtonModule,
    MessageModule,
    RippleModule
  ],
  templateUrl: './login.html', // Pointing to the file above
  styleUrls: ['./login.scss']
})
export class LoginComponent {
  readonly store = inject(AuthStore);
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);

  loginForm = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  });

  onSubmit() {
    if (this.loginForm.invalid) {
      Object.values(this.loginForm.controls).forEach(c => c.markAsTouched());
      return;
    }

    this.store.resetErrors();
    const { email, password } = this.loginForm.getRawValue();
    this.store.login({ email, password });
  }

  getFieldError(fieldName: string): string | null {
    const control = this.loginForm.get(fieldName);
    const serverError = this.store.validationErrors()[fieldName];
  
    // check Server Errors
    if (serverError?.length) {
      return serverError[0];  // Return the first error message
    }

    // Check Local Errors
    if (control && (control.touched || control.dirty) && control.errors) {
      if (control.errors['required']) return `${fieldName.charAt(0).toUpperCase() + fieldName.slice(1)} is required.`;
      if (control.errors['email']) return 'Please enter a valid email address.';
    }

    return null;
  }

  navigateToRegister() {
    this.router.navigate(['/auth/register']);
  }
}