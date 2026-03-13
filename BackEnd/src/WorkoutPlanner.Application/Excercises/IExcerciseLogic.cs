using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.Excercises;

public interface IExcerciseLogic
{
    public Task <Excercise> CreateExcercise(string name);
    public void DeleteExcercise(Excercise excercise);
    public Task<IList<Excercise>> GetAllExcercises();
    public Task<Excercise> GetExcerciseByName(string name);
}