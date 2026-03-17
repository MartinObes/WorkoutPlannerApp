export interface CreateEvaluationRequest {
  playerId: string;
  excerciseId: string;
  reps: number;
  weight: number;
}

export interface DeleteEvaluationRequest {
  evaluationId: string;
}

export interface GetEvaluationsByPlayerIdRequest {
  playerId: string;
}

export interface GetEvaluationsByExerciseIdRequest {
  excerciseId: string;
}

export interface CompareEvaluationsRequest {
  evaluationId1: string;
  evaluationId2: string;
}

export interface CompareEvaluationsResponse {
  difference: number;
}

export interface EvaluationResponse {
  id: string;
  playerId: string;
  excerciseId: string;
  date: string;
  reps: number;
  weight: number;
}

export interface EvaluationsResponse {
  evaluations: EvaluationResponse[];
}
