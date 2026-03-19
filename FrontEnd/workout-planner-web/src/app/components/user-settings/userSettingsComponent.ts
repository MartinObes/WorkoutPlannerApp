import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UserRole } from '../../models/enums';
import { DeleteUserRequest, UpdateUserRequest, UserResponse } from '../../models/user.models';
import { SessionData, SessionService } from '../../services/session.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-user-settings',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-settings.html',
})
export class UserSettingsComponent implements OnInit, OnDestroy {
  userId = '';
  username = '';
  user: UserResponse | null = null;
  error: string | null = null;
  showDeleteModal = false;
  showEditForm = false;

  private readonly destroy$ = new Subject<void>();

  readonly userRoles = [
    { label: 'Player', value: UserRole.Player },
    { label: 'Trainer', value: UserRole.Trainer },
  ];

  constructor(
    private readonly userService: UserService,
    private readonly sessionService: SessionService,
    private readonly router: Router,
    private readonly cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.sessionService.sessionChanges$
      .pipe(takeUntil(this.destroy$))
      .subscribe((session: SessionData | null) => {
        const nextUserId = session?.userId ?? '';
        const nextUserName = session?.userName ?? '';

        if (!nextUserId || !nextUserName) {
          this.userId = '';
          this.username = '';
          this.user = null;
          this.error = 'User session not found.';
          this.cdr.detectChanges();
          return;
        }

        if (this.userId === nextUserId && this.username === nextUserName && this.user) {
          return;
        }

        this.userId = nextUserId;
        this.username = nextUserName;
        this.error = null;
        this.cdr.detectChanges();
        this.loadUser();
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadUser(): void {
    if (!this.username) {
      this.error = 'User session not found.';
      this.user = null;
      this.cdr.detectChanges();
      return;
    }

    this.userService.getByName(this.username).subscribe({
      next: (response) => {
        this.user = response;
        this.cdr.detectChanges();
      },
      error: () => {
        this.user = null;
        this.error = 'Could not load user data.';
        this.cdr.detectChanges();
      },
    });
  }

  openDeleteModal(): void {
    this.showDeleteModal = true;
  }

  closeDeleteModal(): void {
    this.showDeleteModal = false;
  }

  openEditForm(): void {
    this.showEditForm = true;
  }

  closeEditForm(): void {
    this.showEditForm = false;
  }

  onRoleChanged(roleValue: string): void {
    if (!this.user) {
      return;
    }

    this.user.role = Number(roleValue) as UserRole;
  }

  getRoleLabel(role: UserRole | undefined): string {
    if (role === undefined || role === null) {
      return 'Unknown';
    }

    return UserRole[role] ?? String(role);
  }

  confirmEdit(): void {
    if (!this.user || !this.userId || !this.username) {
      this.error = 'Could not update user role.';
      return;
    }

    const updateReq: UpdateUserRequest = {
      userId: this.userId,
      role: this.user.role,
    };

    this.userService.update(this.username, updateReq).subscribe({
      next: (updatedUser) => {
        this.user = updatedUser;
        const roleAsString = UserRole[updatedUser.role] ?? String(updatedUser.role);
        this.sessionService.updateRole(roleAsString);
        this.showEditForm = false;
        this.error = null;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Could not update user role.';
        this.cdr.detectChanges();
      },
    });
  }

  confirmDelete(): void {
    if (!this.username) {
      this.error = 'Could not delete user.';
      return;
    }

    const deleteReq: DeleteUserRequest = {
      name: this.username,
    };

    this.userService.delete(deleteReq).subscribe({
      next: () => {
        this.sessionService.clearSession();
        this.router.navigate(['/login']);
      },
      error: () => {
        this.error = 'Could not delete user.';
        this.cdr.detectChanges();
      },
    });
  }
}
