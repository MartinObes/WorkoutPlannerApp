import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { EvaluationService } from '../../services/evaluation.service';
import { ExerciseService } from '../../services/exercise.service';
import { ExerciseResponse } from '../../models/exercise.models';
import { ExerciseSearchSelectComponent } from '../shared/exercise-search-select/exercise-search-select.component';

@Component({
  selector: 'app-create-evaluation',
  standalone: true,
  imports: [ReactiveFormsModule, RouterModule, ExerciseSearchSelectComponent],
  templateUrl: './create-evaluation.html',
})
export class CreateEvaluationComponent {
  private readonly fb = inject(FormBuilder);
  private readonly evaluationService = inject(EvaluationService);
  private readonly exerciseService = inject(ExerciseService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  userId = '';
  isSubmitting = false;
  errorMessage = '';
  invalidFields: string[] = [];
  excercises: ExerciseResponse[] = [];
  selectedExerciseName = '';
  isExercisesLoading = false;

  readonly form = this.fb.nonNullable.group({
    excerciseId: ['', [Validators.required]],
    reps: [0, [Validators.required, Validators.min(1)]],
    weight: [0, [Validators.required, Validators.min(0)]],
  });

  ngOnInit(): void {
    this.userId =
      this.route.snapshot.paramMap.get('userId') ??
      this.route.parent?.snapshot.paramMap.get('userId') ??
      '';

    this.loadExercises();
  }

  onSubmit(): void {
    if (this.form.invalid || this.isSubmitting) {
      this.form.markAllAsTouched();
      if (!this.isSubmitting) {
        this.invalidFields = this.getInvalidFields();
        this.errorMessage =
          this.invalidFields.length > 0
            ? `Please fix: ${this.invalidFields.join(', ')}`
            : 'Please complete all required fields. Reps must be at least 1 and weight cannot be negative.';
      }
      return;
    }

    this.errorMessage = '';
    this.invalidFields = [];
    this.isSubmitting = true;

    const values = this.form.getRawValue();
    const request = {
      playerId: this.userId,
      excerciseId: values.excerciseId,
      reps: values.reps,
      weight: values.weight,
    };

    this.evaluationService.create(request).subscribe({
      next: () => {
        this.router.navigate(['/app', this.userId, 'evaluations']);
      },
      error: () => {
        this.errorMessage = 'Could not create evaluation. Please try again.';
        this.isSubmitting = false;
      },
    });
  }

  private getInvalidFields(): string[] {
    const labels: Record<string, string> = {
      excerciseId: 'exercise',
      reps: 'reps',
      weight: 'weight',
    };

    return Object.keys(this.form.controls)
      .filter((key) => this.form.get(key)?.invalid)
      .map((key) => labels[key] || key);
  }

  loadExercises(): void {
    this.isExercisesLoading = true;

    this.exerciseService.getAll().subscribe({
      next: (response: any) => {
        this.excercises = response.excercises ?? response.exercises ?? [];
        this.isExercisesLoading = false;
      },
      error: () => {
        console.error('Could not load exercises.');
        this.isExercisesLoading = false;
      },
    });
  }

  onExerciseSelectionChange(exercise: ExerciseResponse | null): void {
    if (!exercise) {
      this.selectedExerciseName = '';
      this.form.controls.excerciseId.setValue('');
      return;
    }

    this.selectedExerciseName = exercise.name;
    this.form.controls.excerciseId.setValue(exercise.id);
    this.form.controls.excerciseId.markAsTouched();
  }

  get exercisePlaceholder(): string {
    if (this.isExercisesLoading) {
      return 'Loading exercises...';
    }

    return 'Search and select exercise...';
  }

  get selectedExerciseId(): string {
    return this.form.controls.excerciseId.value;
  }

  set selectedExerciseId(value: string) {
    this.form.controls.excerciseId.setValue(value);
  }
}
