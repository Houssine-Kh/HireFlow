import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

// PrimeNG Imports
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { MessageModule } from 'primeng/message';
import { SelectModule } from 'primeng/select'
import { RippleModule } from 'primeng/ripple'; 

// Our Store
import { AuthStore } from '../../../infrastructure/auth/auth.store';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    InputTextModule,
    PasswordModule,
    ButtonModule,
    MessageModule,
    SelectModule,
    RippleModule
  ],
  templateUrl: './register.html',
  styleUrls: ['./register.scss'] 
})
export class RegisterComponent {
  readonly store = inject(AuthStore);
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);

  // Role Options
  roles = [
    { label: 'Candidate (Job Seeker)', value: 'Candidate' },
    { label: 'Recruiter (Hiring Manager)', value: 'Recruiter' }
  ];

  registerForm = this.fb.nonNullable.group({
    firstName: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    role: ['Candidate', [Validators.required]] // Default to Candidate
  });

  onSubmit() {
    if (this.registerForm.invalid) {
      Object.values(this.registerForm.controls).forEach(c => c.markAsTouched());
      return;
    }

    this.store.resetErrors();
    const { firstName, lastName, email, password, role } = this.registerForm.getRawValue();
    
    // Dispatch register action
    this.store.register({ firstName, lastName, email, password, role });
  }

  getFieldError(fieldName: string): string | null {
    const control = this.registerForm.get(fieldName);
    const serverError = this.store.validationErrors()[fieldName];
    
    // Check Server Errors
    if (serverError && serverError.length > 0) {
      return serverError[0];
    }

    // Check Local Errors
    if (control && (control.touched || control.dirty) && control.errors) {
      if (control.errors['required']) return `${this.formatFieldName(fieldName)} is required.`;
      if (control.errors['email']) return 'Please enter a valid email address.';
      if (control.errors['minlength']) return 'Password must be at least 6 characters.';
    }

    return null;
  }

  // Helper to format "firstName" -> "First Name"
  private formatFieldName(name: string): string {
    const spaced = name.replace(/([A-Z])/g, ' $1').trim();
    return spaced.charAt(0).toUpperCase() + spaced.slice(1);
  }

  navigateToLogin() {
    this.router.navigate(['/auth/login']);
  }
}