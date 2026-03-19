import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { EvaluationResponse } from '../../models/evaluation.models';
import { EvaluationService } from '../../services/evaluation.service';
import { ExerciseResponse } from '../../models/exercise.models';
import { ExerciseService } from '../../services/exercise.service';
import { SessionData, SessionService } from '../../services/session.service';
import { FormsModule } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ExerciseSearchSelectComponent } from '../shared/exercise-search-select/exercise-search-select.component';

@Component({
  selector: 'app-evaluation-manager',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, ExerciseSearchSelectComponent],
  templateUrl: './evaluation-manager.html',
})
export class EvaluationManager implements OnInit, OnDestroy {
  userId = '';
  allEvaluations: EvaluationResponse[] = [];
  filteredEvaluations: EvaluationResponse[] = [];
  exercises: ExerciseResponse[] = [];
  selectedExerciseId = '';
  showDeleteModal = false;
  pendingDeleteEvaluationId: string | null = null;
  isDeleting = false;
  loading = false;
  error: string | null = null;
  private readonly destroy$ = new Subject<void>();

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly exerciseService: ExerciseService,
    private readonly evaluationService: EvaluationService,
    private readonly sessionService: SessionService,
    private readonly cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.loadExercises();

    this.sessionService.sessionChanges$
      .pipe(takeUntil(this.destroy$))
      .subscribe((session: SessionData | null) => {
        const nextUserId = session?.userId ?? '';

        if (!nextUserId) {
          this.userId = '';
          this.allEvaluations = [];
          this.filteredEvaluations = [];
          this.loading = false;
          this.error = 'User session not found.';
          this.cdr.detectChanges();
          return;
        }

        if (this.userId === nextUserId && this.allEvaluations.length > 0) {
          return;
        }

        this.userId = nextUserId;
        this.error = null;
        this.cdr.detectChanges();
        this.loadEvaluations();
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadExercises(): void {
    this.exerciseService.getAll().subscribe({
      next: (response: any) => {
        this.exercises = response.excercises ?? response.exercises ?? [];
        this.cdr.detectChanges();
      },
      error: () => {
        console.error('Could not load exercises.');
        this.cdr.detectChanges();
      },
    });
  }

  loadEvaluations(): void {
    if (!this.userId) {
      this.loading = false;
      this.error = 'User session not found.';
      this.cdr.detectChanges();
      return;
    }

    this.loading = true;
    this.error = null;
    this.cdr.detectChanges();

    this.evaluationService.getByPlayerId(this.userId).subscribe({
      next: (response) => {
        this.allEvaluations = response.evaluations ?? [];
        this.applyFilter();
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Could not load evaluations.';
        this.loading = false;
        this.cdr.detectChanges();
      },
    });
  }

  applyFilter(): void {
    if (!this.selectedExerciseId) {
      this.filteredEvaluations = this.allEvaluations;
    } else {
      this.filteredEvaluations = this.allEvaluations.filter(
        (e) => e.excerciseId === this.selectedExerciseId,
      );
    }
  }

  onExerciseSelectionChange(exercise: ExerciseResponse | null): void {
    if (!exercise) {
      this.selectedExerciseId = '';
      this.applyFilter();
      return;
    }

    this.selectedExerciseId = exercise.id;
    this.applyFilter();
  }

  goToCreateEvaluation(): void {
    this.router.navigate(['create'], { relativeTo: this.route });
  }

  get selectedExerciseName(): string {
    if (!this.selectedExerciseId) {
      return '';
    }

    return this.exercises.find((exercise) => exercise.id === this.selectedExerciseId)?.name ?? '';
  }

  getExerciseName(exerciseId: string): string {
    return this.exercises.find((e) => e.id === exerciseId)?.name ?? 'Unknown exercise';
  }

  openDeleteModal(evaluationId: string): void {
    this.pendingDeleteEvaluationId = evaluationId;
    this.showDeleteModal = true;
  }

  closeDeleteModal(): void {
    if (this.isDeleting) {
      return;
    }

    this.showDeleteModal = false;
    this.pendingDeleteEvaluationId = null;
  }

  confirmDeleteEvaluation(): void {
    if (!this.pendingDeleteEvaluationId || this.isDeleting) {
      return;
    }

    const evaluationId = this.pendingDeleteEvaluationId;
    this.isDeleting = true;

    this.evaluationService.delete(evaluationId).subscribe({
      next: () => {
        this.allEvaluations = this.allEvaluations.filter((e) => e.id !== evaluationId);
        this.applyFilter();
        this.isDeleting = false;
        this.showDeleteModal = false;
        this.pendingDeleteEvaluationId = null;
      },
      error: () => {
        this.error = 'Could not delete evaluation.';
        this.isDeleting = false;
      },
    });
  }
}
