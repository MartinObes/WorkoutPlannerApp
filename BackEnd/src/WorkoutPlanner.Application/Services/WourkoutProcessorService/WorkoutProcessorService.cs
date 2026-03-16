using WorkoutPlanner.Application.Evaluations;
using WorkoutPlanner.Application.Users;
using WorkoutPlanner.Application.Workouts;
using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.Services.WourkoutProcessorService;

public class WorkoutProcessorService(IEvaluationLogic evaluationLogic, IWorkoutLogic workoutLogic, IUserLogic userLogic) : IWorkoutProcessorService
{
    private readonly IEvaluationLogic _evaluationLogic = evaluationLogic ?? throw new ArgumentNullException(nameof(evaluationLogic));
    private readonly IWorkoutLogic _workoutLogic = workoutLogic ?? throw new ArgumentNullException(nameof(workoutLogic));
    private readonly IUserLogic _userLogic = userLogic ?? throw new ArgumentNullException(nameof(userLogic));
    
    public async void ProcessWorkout(string workoutName, string username)
    {
        var workout = await _workoutLogic.GetWorkoutByName(workoutName);
        
        foreach(var exercise in workout.WorkoutExcercises)
        {
            if(exercise.LoadType == Enums.LoadType.Percentage)
            {
                var user = await _userLogic.GetUserByName(username);
                var evaluations = await _evaluationLogic.GetEvaluationsByPlayerId(user.Id);
                var exerciseEvals = evaluations.Where(e => e.ExcerciseId == exercise.ExcerciseId);
                
            }
        }
        
    }
}