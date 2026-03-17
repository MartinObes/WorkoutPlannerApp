export interface CreateEvaluationRequest {
  playerId: string;
  excerciseId: string;
  reps: number;
  weight: number;
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
