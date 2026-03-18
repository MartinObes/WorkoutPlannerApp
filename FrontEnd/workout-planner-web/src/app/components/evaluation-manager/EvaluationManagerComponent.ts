import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { EvaluationResponse } from '../../models/evaluation.models';
import { EvaluationService } from '../../services/evaluation.service';
import { ExerciseResponse } from '../../models/exercise.models';
import { ExerciseService } from '../../services/exercise.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-evaluation-manager',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './evaluation-manager.html',
})
export class EvaluationManager implements OnInit {
  userId = '';
  allEvaluations: EvaluationResponse[] = [];
  filteredEvaluations: EvaluationResponse[] = [];
  exercises: ExerciseResponse[] = [];
  selectedExerciseId = '';
  exerciseSearchTerm = '';
  showExerciseDropdown = false;
  loading = false;
  error: string | null = null;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly exerciseService: ExerciseService,
    private readonly evaluationService: EvaluationService,
  ) {}

  ngOnInit(): void {
    this.userId =
      this.route.snapshot.paramMap.get('userId') ??
      this.route.parent?.snapshot.paramMap.get('userId') ??
      '';

    this.loadExercises();
    this.loadEvaluations();
  }

  loadExercises(): void {
    this.exerciseService.getAll().subscribe({
      next: (response: any) => {
        this.exercises = response.excercises ?? response.exercises ?? [];
      },
      error: () => {
        console.error('Could not load exercises.');
      },
    });
  }

  loadEvaluations(): void {
    if (!this.userId) {
      this.loading = false;
      this.error = 'Missing user id in route.';
      return;
    }

    this.loading = true;
    this.error = null;

    this.evaluationService.getByPlayerId(this.userId).subscribe({
      next: (response) => {
        this.allEvaluations = response.evaluations;
        this.applyFilter();
        this.loading = false;
      },
      error: () => {
        this.error = 'Could not load evaluations.';
        this.loading = false;
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

  onExerciseSearch(value: string): void {
    this.exerciseSearchTerm = value;
    this.showExerciseDropdown = true;

    if (!value.trim()) {
      this.selectedExerciseId = '';
      this.applyFilter();
    }
  }

  selectExercise(exercise: ExerciseResponse): void {
    this.selectedExerciseId = exercise.id;
    this.exerciseSearchTerm = exercise.name;
    this.showExerciseDropdown = false;
    this.applyFilter();
  }

  selectAllExercises(): void {
    this.selectedExerciseId = '';
    this.exerciseSearchTerm = '';
    this.showExerciseDropdown = false;
    this.applyFilter();
  }

  hideExerciseDropdown(): void {
    setTimeout(() => {
      this.showExerciseDropdown = false;
    }, 100);
  }

  goToCreateEvaluation(): void {
    this.router.navigate(['create'], { relativeTo: this.route });
  }

  get filteredExercises(): ExerciseResponse[] {
    const term = this.exerciseSearchTerm.trim().toLowerCase();
    if (!term) return this.exercises;
    return this.exercises.filter((e) => e.name.toLowerCase().includes(term));
  }

  getExerciseName(exerciseId: string): string {
    return this.exercises.find((e) => e.id === exerciseId)?.name ?? 'Unknown exercise';
  }
}
