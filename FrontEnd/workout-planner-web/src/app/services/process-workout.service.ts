import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  ProcessWorkoutRequest,
  ProcessedWorkoutResponse,
} from '../models/workout-processing.models';

@Injectable({ providedIn: 'root' })
export class ProcessWorkoutService {
  private readonly baseUrl = `${environment.apiUrl}/process-workout`;

  constructor(private http: HttpClient) {}

  processWorkout(request: ProcessWorkoutRequest): Observable<ProcessedWorkoutResponse> {
    return this.http.post<ProcessedWorkoutResponse>(this.baseUrl, request);
  }
}
