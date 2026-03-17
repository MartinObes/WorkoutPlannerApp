import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  CreateExerciseRequest,
  ExerciseResponse,
  GetAllExercisesResponse,
} from '../models/exercise.models';

@Injectable({ providedIn: 'root' })
export class ExerciseService {
  private readonly baseUrl = `${environment.apiUrl}/exercises`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<GetAllExercisesResponse> {
    return this.http.get<GetAllExercisesResponse>(this.baseUrl);
  }

  getByName(name: string): Observable<ExerciseResponse> {
    return this.http.get<ExerciseResponse>(`${this.baseUrl}/${name}`);
  }

  create(request: CreateExerciseRequest): Observable<ExerciseResponse> {
    return this.http.post<ExerciseResponse>(this.baseUrl, request);
  }

  delete(exerciseId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${exerciseId}`);
  }
}
