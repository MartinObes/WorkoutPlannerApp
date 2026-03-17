import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  UpdateWorkoutExerciseRequest,
  WorkoutExerciseResponse,
  WorkoutExercisesResponse,
} from '../models/workout-exercise.models';

@Injectable({ providedIn: 'root' })
export class WorkoutExerciseService {
  private readonly baseUrl = `${environment.apiUrl}/workout-excercises`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<WorkoutExercisesResponse> {
    return this.http.get<WorkoutExercisesResponse>(this.baseUrl);
  }

  getById(workoutExcerciseId: string): Observable<WorkoutExerciseResponse> {
    return this.http.get<WorkoutExerciseResponse>(`${this.baseUrl}/${workoutExcerciseId}`);
  }

  update(request: UpdateWorkoutExerciseRequest): Observable<WorkoutExerciseResponse> {
    return this.http.put<WorkoutExerciseResponse>(
      `${this.baseUrl}/${request.workoutExcerciseId}`,
      request,
    );
  }

  delete(workoutExcerciseId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${workoutExcerciseId}`);
  }
}
