import { CommonModule } from '@angular/common';
import {
  Component,
  ElementRef,
  EventEmitter,
  HostListener,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';
import { ExerciseResponse } from '../../../models/exercise.models';

@Component({
  selector: 'app-exercise-search-select',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './exercise-search-select.component.html',
})
export class ExerciseSearchSelectComponent implements OnChanges {
  constructor(private readonly elementRef: ElementRef<HTMLElement>) {}

  @Input() exercises: ExerciseResponse[] = [];
  @Input() placeholder = 'Search and select exercise...';
  @Input() loading = false;
  @Input() selectedExerciseId = '';
  @Input() selectedExerciseName = '';
  @Input() showAllOption = false;
  @Input() allOptionLabel = 'All Exercises';
  @Input() clearTextOnFocus = false;
  @Input() clearSelectionOnFocus = false;

  @Output() selectionChange = new EventEmitter<ExerciseResponse | null>();

  searchTerm = '';
  showDropdown = false;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['selectedExerciseName']) {
      this.searchTerm = this.selectedExerciseName ?? '';
    }
  }

  get filteredExercises(): ExerciseResponse[] {
    const term = this.searchTerm.trim().toLowerCase();
    if (!term) {
      return this.exercises;
    }

    return this.exercises.filter((exercise) => exercise.name.toLowerCase().includes(term));
  }

  onSearchInput(value: string): void {
    this.searchTerm = value;
    this.showDropdown = true;

    if (!value.trim()) {
      this.selectionChange.emit(null);
    }
  }

  onFocus(): void {
    if (this.clearTextOnFocus) {
      this.searchTerm = '';

      if (this.clearSelectionOnFocus) {
        this.selectionChange.emit(null);
      }
    }

    this.showDropdown = true;
  }

  onBlur(): void {
    setTimeout(() => {
      this.showDropdown = false;
    }, 100);
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const target = event.target as Node | null;

    if (!target) {
      return;
    }

    if (!this.elementRef.nativeElement.contains(target)) {
      this.showDropdown = false;
    }
  }

  selectExercise(exercise: ExerciseResponse): void {
    this.searchTerm = exercise.name;
    this.showDropdown = false;
    this.selectionChange.emit(exercise);
  }

  selectAll(): void {
    this.searchTerm = '';
    this.showDropdown = false;
    this.selectionChange.emit(null);
  }
}
