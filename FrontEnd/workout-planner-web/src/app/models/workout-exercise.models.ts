import { LoadType } from './enums';

export interface CreateWorkoutExerciseRequest {
  workoutId: string;
  excerciseId: string;
  reps: number;
  sets: number;
  loadType: LoadType;
  weight?: number;
  percentage?: number;
}

export interface UpdateWorkoutExerciseRequest {
  workoutExcerciseId: string;
  name: string;
  workoutId: string;
  excerciseId: string;
  reps: number;
  sets: number;
  loadType: LoadType;
  weight?: number;
  percentage?: number;
}

export interface DeleteWorkoutExerciseRequest {
  workoutExcerciseId: string;
}

export interface GetWorkoutExerciseByIdRequest {
  workoutExcerciseId: string;
}

export interface WorkoutExerciseResponse {
  id: string;
  workoutId: string;
  excerciseId: string;
  excerciseName: string;
  reps: number;
  sets: number;
  loadType: LoadType;
  weight?: number;
  percentage?: number;
}

export interface WorkoutExercisesResponse {
  workoutExcercises: WorkoutExerciseResponse[];
}
