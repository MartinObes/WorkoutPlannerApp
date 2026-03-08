using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.Excercises;

public interface IExcerciseLogic
{
    public Excercise CreateExcercise(string name);
    public void DeleteExcercise(Excercise excercise);
}