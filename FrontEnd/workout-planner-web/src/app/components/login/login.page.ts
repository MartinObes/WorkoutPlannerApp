import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UserRole } from '../../models/enums';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-login-page',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.page.html',
})
export class LoginPage {
  private readonly fb = inject(FormBuilder);
  private readonly userService = inject(UserService);
  private readonly router = inject(Router);

  errorMessage = '';
  isSubmitting = false;

  readonly form = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });

  onSubmit(): void {
    if (this.form.invalid || this.isSubmitting) {
      this.form.markAllAsTouched();
      return;
    }

    this.errorMessage = '';
    this.isSubmitting = true;

    this.userService.login(this.form.getRawValue()).subscribe({
      next: (user) => {
        this.isSubmitting = false;
        const roleAsString = UserRole[user.role] ?? String(user.role);
        localStorage.setItem('currentUserName', user.name);
        localStorage.setItem('currentUserId', user.id);
        localStorage.setItem('currentUserRole', roleAsString);
        this.router.navigate(['/app', user.id]);
      },
      error: () => {
        this.isSubmitting = false;
        this.errorMessage = 'Invalid email or password.';
      },
    });
  }
}
