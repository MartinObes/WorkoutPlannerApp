using WorkoutPlanner.Application.Evaluations;
using WorkoutPlanner.Application.Users;
using WorkoutPlanner.Application.Workouts;
using WorkoutPlanner.Domain;
using WorkoutPlanner.Domain.AuxiliaryDomainClasses;

namespace WorkoutPlanner.Application.Services.WourkoutProcessorService;

public class WorkoutProcessorService(IEvaluationLogic evaluationLogic, IWorkoutLogic workoutLogic, IUserLogic userLogic) : IWorkoutProcessorService
{
    private readonly IEvaluationLogic _evaluationLogic = evaluationLogic ?? throw new ArgumentNullException(nameof(evaluationLogic));
    private readonly IWorkoutLogic _workoutLogic = workoutLogic ?? throw new ArgumentNullException(nameof(workoutLogic));
    private readonly IUserLogic _userLogic = userLogic ?? throw new ArgumentNullException(nameof(userLogic));

    public async Task<List<ProcessedExercise>> ProcessWorkout(string workoutName, string username)
    {
        var workout = await _workoutLogic.GetWorkoutByName(workoutName);
        var user = await _userLogic.GetUserByName(username);
        var evaluations = await _evaluationLogic.GetEvaluationsByPlayerId(user.Id);

        var processedExercises = new List<ProcessedExercise>();

        foreach (var exercise in workout.WorkoutExcercises)
        {
            int? calculatedWeight = null;

            if (exercise.LoadType == Enums.LoadType.Percentage)
            {
                var exerciseEvals = evaluations.Where(e => e.ExcerciseId == exercise.ExcerciseId).ToList();

                if (exerciseEvals.Any())
                {
                    var latestEval = exerciseEvals.OrderByDescending(e => e.Date).First();
                    calculatedWeight = (int)Math.Round(exercise.Percentage!.Value / 100.0 * latestEval.Weight);
                }
            }
            else
            {
                calculatedWeight = exercise.Weight!.Value;
            }
            
            var processedExercise = new ProcessedExercise
            {
                ExerciseName = exercise.Excercise.Name,
                Sets = exercise.Sets,
                Reps = exercise.Reps,
                CalculatedWeight = calculatedWeight,
                LoadType = exercise.LoadType,
                OriginalPercentage = exercise.Percentage
            };

            processedExercises.Add(processedExercise);
        }

        return processedExercises;
    }
}