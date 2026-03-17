export interface CreateExerciseRequest {
  name: string;
}

export interface DeleteExerciseRequest {
  name: string;
}

export interface GetExerciseByNameRequest {
  name: string;
}

export interface ExerciseResponse {
  id: string;
  name: string;
}

export interface GetAllExercisesResponse {
  excercises: ExerciseResponse[];
}
