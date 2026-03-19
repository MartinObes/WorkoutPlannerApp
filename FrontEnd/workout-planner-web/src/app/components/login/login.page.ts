import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UserRole } from '../../models/enums';
import { UserService } from '../../services/user.service';
import { SessionService } from '../../services/session.service';

@Component({
  selector: 'app-login-page',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.page.html',
})
export class LoginPage {
  constructor(
    private readonly userService: UserService,
    private readonly sessionService: SessionService,
    private readonly router: Router,
  ) {}

  errorMessage = '';
  isSubmitting = false;

  readonly form = new FormGroup({
    email: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required, Validators.email],
    }),
    password: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required],
    }),
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

        this.sessionService.setSession({
          userId: user.id,
          userName: user.name,
          userRole: roleAsString,
        });

        this.router.navigate(['/app']);
      },
      error: () => {
        this.isSubmitting = false;
        this.errorMessage = 'Invalid email or password.';
      },
    });
  }
}
