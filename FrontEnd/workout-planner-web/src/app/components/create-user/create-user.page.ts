import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UserRole } from '../../models/enums';
import { CreateUserRequest } from '../../models/user.models';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-create-user-page',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './create-user.page.html',
})
export class CreateUserPage {
  constructor(
    private readonly userService: UserService,
    private readonly router: Router,
  ) {}

  errorMessage = '';
  isSubmitting = false;

  readonly userRoles = [
    { label: 'Player', value: UserRole.Player },
    { label: 'Trainer', value: UserRole.Trainer },
  ];

  readonly form = new FormGroup({
    name: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required, Validators.minLength(2)],
    }),
    surname: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required, Validators.minLength(2)],
    }),
    email: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required, Validators.email],
    }),
    password: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required, Validators.minLength(8)],
    }),
    role: new FormControl(UserRole.Player, {
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

    const request: CreateUserRequest = this.form.getRawValue();

    this.userService.create(request).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.router.navigate(['/login']);
      },
      error: () => {
        this.isSubmitting = false;
        this.errorMessage = 'Could not create user. Please try again.';
      },
    });
  }
}
