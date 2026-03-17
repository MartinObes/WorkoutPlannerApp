import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  CompareEvaluationsResponse,
  CreateEvaluationRequest,
  EvaluationResponse,
  EvaluationsResponse,
} from '../models/evaluation.models';

@Injectable({ providedIn: 'root' })
export class EvaluationService {
  private readonly baseUrl = `${environment.apiUrl}/evaluations`;

  constructor(private http: HttpClient) {}

  create(request: CreateEvaluationRequest): Observable<EvaluationResponse> {
    return this.http.post<EvaluationResponse>(this.baseUrl, request);
  }

  delete(evaluationId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${evaluationId}`);
  }

  getById(evaluationId: string): Observable<EvaluationResponse> {
    return this.http.get<EvaluationResponse>(`${this.baseUrl}/${evaluationId}`);
  }

  getByPlayerId(playerId: string): Observable<EvaluationsResponse> {
    return this.http.get<EvaluationsResponse>(`${this.baseUrl}/by-player/${playerId}`);
  }

  getByExerciseId(excerciseId: string): Observable<EvaluationsResponse> {
    return this.http.get<EvaluationsResponse>(`${this.baseUrl}/by-exercise/${excerciseId}`);
  }

  compareEvaluations(
    evaluationId1: string,
    evaluationId2: string,
  ): Observable<CompareEvaluationsResponse> {
    return this.http.get<CompareEvaluationsResponse>(`${this.baseUrl}/compare`, {
      params: {
        evaluationId1,
        evaluationId2,
      },
    });
  }
}
