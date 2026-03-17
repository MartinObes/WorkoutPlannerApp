import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  CreateWorkoutRequest,
  UpdateWorkoutRequest,
  WorkoutResponse,
  WorkoutsResponse,
} from '../models/workout.models';

@Injectable({ providedIn: 'root' })
export class WorkoutService {
  private readonly baseUrl = `${environment.apiUrl}/workouts`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<WorkoutsResponse> {
    return this.http.get<WorkoutsResponse>(this.baseUrl);
  }

  getByName(name: string): Observable<WorkoutResponse> {
    return this.http.get<WorkoutResponse>(`${this.baseUrl}/${name}`);
  }

  create(request: CreateWorkoutRequest): Observable<WorkoutResponse> {
    return this.http.post<WorkoutResponse>(this.baseUrl, request);
  }

  update(request: UpdateWorkoutRequest): Observable<WorkoutResponse> {
    return this.http.put<WorkoutResponse>(this.baseUrl, request);
  }

  delete(name: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${name}`);
  }
}
