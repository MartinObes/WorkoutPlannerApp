import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LoadType } from '../../models/enums';
import { CreateWorkoutExerciseArgRequest, CreateWorkoutRequest } from '../../models/workout.models';
import { WorkoutService } from '../../services/workout.service';
import { SessionService } from '../../services/session.service';
import { ExerciseService } from '../../services/exercise.service';
import { ExerciseResponse } from '../../models/exercise.models';
import { ExerciseSearchSelectComponent } from '../shared/exercise-search-select/exercise-search-select.component';

@Component({
  selector: 'app-create-workout',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ExerciseSearchSelectComponent],
  templateUrl: './create-workout.html',
})
export class CreateWorkoutComponent implements OnInit {
  constructor(
    private readonly workoutService: WorkoutService,
    private readonly sessionService: SessionService,
    private readonly exerciseService: ExerciseService,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly cdr: ChangeDetectorRef,
  ) {}

  userId = '';
  username = '';
  userRole = '';
  exercises: ExerciseResponse[] = [];
  isSubmitting = false;
  error: string | null = null;
  expandedExerciseIndex = 0;

  readonly loadTypeOptions = [
    { label: 'Weight', value: LoadType.Weight },
    { label: 'Percentage', value: LoadType.Percentage },
  ];

  readonly form = new FormGroup({
    name: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required, Validators.minLength(2)],
    }),
    workoutExcercises: new FormArray<FormGroup>([]),
  });

  ngOnInit(): void {
    const session = this.sessionService.getSession();
    if (!session) {
      this.error = 'User session not found.';
      return;
    }

    this.userId = session.userId;
    this.username = session.userName;
    this.userRole = session.userRole;
    this.addWorkoutExercise();
    this.loadExercises();
  }

  get isTrainer(): boolean {
    return this.userRole.trim().toLowerCase() === 'trainer';
  }

  get workoutExercisesArray(): FormArray<FormGroup> {
    return this.form.controls.workoutExcercises;
  }

  addWorkoutExercise(): void {
    const group = new FormGroup({
      excerciseId: new FormControl('', {
        nonNullable: true,
        validators: [Validators.required],
      }),
      sets: new FormControl(1, {
        nonNullable: true,
        validators: [Validators.required, Validators.min(1)],
      }),
      reps: new FormControl(1, {
        nonNullable: true,
        validators: [Validators.required, Validators.min(1)],
      }),
      loadType: new FormControl(LoadType.Weight, {
        nonNullable: true,
        validators: [Validators.required],
      }),
      weight: new FormControl<number | null>(0, {
        validators: [Validators.required, Validators.min(0)],
      }),
      percentage: new FormControl<number | null>(null),
    });

    this.workoutExercisesArray.push(group);
    this.expandedExerciseIndex = this.workoutExercisesArray.length - 1;
  }

  removeWorkoutExercise(index: number): void {
    if (this.workoutExercisesArray.length <= 1) {
      return;
    }

    this.workoutExercisesArray.removeAt(index);

    if (this.expandedExerciseIndex === index) {
      this.expandedExerciseIndex = Math.max(0, index - 1);
      return;
    }

    if (this.expandedExerciseIndex > index) {
      this.expandedExerciseIndex -= 1;
    }
  }

  expandExercise(index: number): void {
    this.expandedExerciseIndex = index;
  }

  collapseExercise(index: number): void {
    if (this.expandedExerciseIndex === index) {
      this.expandedExerciseIndex = -1;
    }
  }

  isExpanded(index: number): boolean {
    return this.expandedExerciseIndex === index;
  }

  getExerciseName(exerciseId: string): string {
    const selectedExercise = this.exercises.find((exercise) => exercise.id === exerciseId);
    return selectedExercise?.name ?? 'Exercise not selected';
  }

  getLoadType(index: number): LoadType {
    const rawValue = this.workoutExercisesArray.at(index).get('loadType')?.value;
    return Number(rawValue) as LoadType;
  }

  isLoadType(index: number, type: LoadType): boolean {
    return this.getLoadType(index) === type;
  }

  getSelectedExerciseName(index: number): string {
    const group = this.workoutExercisesArray.at(index);
    const exerciseId = String(group.get('excerciseId')?.value ?? '');
    return this.getExerciseName(exerciseId);
  }

  onExerciseSelectionChange(index: number, exercise: ExerciseResponse | null): void {
    const group = this.workoutExercisesArray.at(index);
    const exerciseControl = group.get('excerciseId');

    if (!exerciseControl) {
      return;
    }

    if (!exercise) {
      exerciseControl.setValue('');
      exerciseControl.markAsTouched();
      return;
    }

    exerciseControl.setValue(exercise.id);
    exerciseControl.markAsTouched();
  }

  onLoadTypeChange(index: number): void {
    const group = this.workoutExercisesArray.at(index);
    const loadType = Number(group.get('loadType')?.value) as LoadType;
    const weightControl = group.get('weight');
    const percentageControl = group.get('percentage');

    if (!weightControl || !percentageControl) {
      return;
    }

    if (loadType === LoadType.Weight) {
      weightControl.setValidators([Validators.required, Validators.min(0)]);
      percentageControl.clearValidators();
      percentageControl.setValue(null);
    } else {
      percentageControl.setValidators([
        Validators.required,
        Validators.min(1),
        Validators.max(100),
      ]);
      weightControl.clearValidators();
      weightControl.setValue(null);
    }

    weightControl.updateValueAndValidity();
    percentageControl.updateValueAndValidity();
  }

  loadExercises(): void {
    this.exerciseService.getAll().subscribe({
      next: (response) => {
        this.exercises = response.excercises ?? (response as any).exercises ?? [];
        this.cdr.detectChanges();
      },
      error: (error) => {
        this.error = error?.message ?? 'Could not load exercises.';
        this.cdr.detectChanges();
      },
    });
  }

  onSubmit(): void {
    if (this.form.invalid || this.workoutExercisesArray.length === 0 || this.isSubmitting) {
      this.form.markAllAsTouched();
      if (this.workoutExercisesArray.length === 0) {
        this.error = 'Add at least one workout exercise.';
      }
      return;
    }

    const hasInvalidLoadValues = this.workoutExercisesArray.controls.some((group) => {
      const loadType = Number(group.get('loadType')?.value) as LoadType;
      const weight = group.get('weight')?.value;
      const percentage = group.get('percentage')?.value;

      if (loadType === LoadType.Weight) {
        return weight === null || Number(weight) < 0;
      }

      return percentage === null || Number(percentage) < 1 || Number(percentage) > 100;
    });

    if (hasInvalidLoadValues) {
      this.form.markAllAsTouched();
      this.error =
        'Each workout exercise must include weight for Weight load type, or percentage for Percentage load type.';
      return;
    }

    this.isSubmitting = true;
    this.error = null;

    const values = this.form.getRawValue();
    const workoutExcercises: CreateWorkoutExerciseArgRequest[] = values.workoutExcercises.map(
      (item) => {
        const loadType = Number(item['loadType']) as LoadType;

        const baseRequest: CreateWorkoutExerciseArgRequest = {
          excerciseId: String(item['excerciseId']),
          sets: Number(item['sets']),
          reps: Number(item['reps']),
          loadType,
        };

        if (loadType === LoadType.Weight) {
          return {
            ...baseRequest,
            weight: Number(item['weight']),
          };
        }

        return {
          ...baseRequest,
          percentage: Number(item['percentage']),
        };
      },
    );

    const request: CreateWorkoutRequest = {
      name: values.name,
      coachId: this.isTrainer ? this.userId : undefined,
      workoutExcercises,
      workoutExcerciseArgsList: workoutExcercises,
    };

    this.workoutService.create(request).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.onCancel();
      },
      error: (error) => {
        this.isSubmitting = false;
        this.error = error?.message ?? 'Could not create workout. Please try again.';
        this.cdr.detectChanges();
      },
    });
  }

  onCancel(): void {
    this.router.navigate(['../'], { relativeTo: this.route });
  }
}
