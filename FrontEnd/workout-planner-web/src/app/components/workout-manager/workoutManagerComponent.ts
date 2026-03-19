import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { WorkoutResponse } from '../../models/workout.models';
import { SessionService } from '../../services/session.service';
import { WorkoutService } from '../../services/workout.service';

@Component({
  selector: 'app-workout-manager',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './workout-manager.html',
})
export class WorkoutManagerComponent implements OnInit {
  constructor(
    private readonly workoutService: WorkoutService,
    private readonly sessionService: SessionService,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly cdr: ChangeDetectorRef,
  ) {}

  userId = '';
  username = '';
  workouts: WorkoutResponse[] = [];
  loading = false;
  error: string | null = null;

  ngOnInit(): void {
    const session = this.sessionService.getSession();
    if (!session) {
      this.error = 'User session not found.';
      return;
    }
    this.userId = session.userId;
    this.username = session.userName;
    this.loadWorkouts();
  }

  loadWorkouts(): void {
    this.loading = true;
    this.error = null;

    this.workoutService.getAll().subscribe({
      next: (response) => {
        this.workouts = response.workouts ?? [];
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        this.loading = false;
        this.error = error?.message ?? 'Could not load workouts.';
        this.cdr.detectChanges();
      },
    });
  }

  goToCreateWorkout(): void {
    this.router.navigate(['/app/workouts/create']);
  }

  accessWorkout(workout: WorkoutResponse): void {
    this.router.navigate([workout.id], { relativeTo: this.route });
  }
}
