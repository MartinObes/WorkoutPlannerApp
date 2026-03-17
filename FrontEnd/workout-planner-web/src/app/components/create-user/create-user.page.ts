import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UserRole } from '../../models/enums';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-create-user-page',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './create-user.page.html',
})
export class CreateUserPage {
  private readonly fb = inject(FormBuilder);
  private readonly userService = inject(UserService);
  private readonly router = inject(Router);

  readonly userRoles = [
    { label: 'Player', value: UserRole.Player },
    { label: 'Trainer', value: UserRole.Trainer },
  ];

  isSubmitting = false;
  errorMessage = '';
  invalidFields: string[] = [];

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required]],
    surname: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    role: [UserRole.Player, [Validators.required]],
  });

  onSubmit(): void {
    if (this.form.invalid || this.isSubmitting) {
      this.form.markAllAsTouched();
      if (!this.isSubmitting) {
        this.invalidFields = this.getInvalidFields();
        this.errorMessage =
          this.invalidFields.length > 0
            ? `Please fix: ${this.invalidFields.join(', ')}`
            : 'Please complete all required fields. Password must be at least 8 characters.';
      }
      return;
    }

    this.errorMessage = '';
    this.invalidFields = [];
    this.isSubmitting = true;

    const values = this.form.getRawValue();
    const request = {
      name: values.name.trim(),
      surname: values.surname.trim(),
      email: values.email.trim(),
      password: values.password,
      role: Number(values.role),
    };

    this.userService.create(request).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.router.navigate(['/login']);
      },
      error: () => {
        this.isSubmitting = false;
        this.errorMessage = 'Could not create user. Please verify the input values.';
      },
    });
  }

  private getInvalidFields(): string[] {
    const labels: Record<string, string> = {
      name: 'name',
      surname: 'surname',
      email: 'email',
      password: 'password',
      role: 'role',
    };

    return Object.keys(this.form.controls)
      .filter((key) => this.form.controls[key as keyof typeof this.form.controls].invalid)
      .map((key) => labels[key] ?? key);
  }
}
