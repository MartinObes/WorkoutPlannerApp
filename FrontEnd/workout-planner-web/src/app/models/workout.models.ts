import { LoadType } from './enums';
import { WorkoutExerciseResponse } from './workout-exercise.models';

export interface CreateWorkoutExerciseArgRequest {
  excerciseId: string;
  sets: number;
  reps: number;
  loadType: LoadType;
  weight?: number;
  percentage?: number;
}

export interface CreateWorkoutRequest {
  name: string;
  coachId?: string;
  workoutExcercises: CreateWorkoutExerciseArgRequest[];
  workoutExcerciseArgsList?: CreateWorkoutExerciseArgRequest[];
}

export interface UpdateWorkoutRequest {
  workoutId: string;
  name: string;
  coachId?: string;
}

export interface WorkoutResponse {
  id: string;
  name: string;
  coachId?: string;
  workoutExcercises: WorkoutExerciseResponse[];
}

export interface WorkoutsResponse {
  workouts: WorkoutResponse[];
}
