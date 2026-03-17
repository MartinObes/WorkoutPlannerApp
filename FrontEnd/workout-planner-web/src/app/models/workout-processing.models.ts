import { LoadType } from './enums';

export interface ProcessWorkoutRequest {
  workoutName: string;
  username: string;
}

export interface ProcessedExerciseResponse {
  exerciseName: string;
  sets: number;
  reps: number;
  calculatedWeight?: number;
  loadType: LoadType;
  originalPercentage?: number;
}

export interface ProcessedWorkoutResponse {
  exercises: ProcessedExerciseResponse[];
}
