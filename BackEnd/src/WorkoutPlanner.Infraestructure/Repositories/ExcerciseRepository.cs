using WorkoutPlanner.Application.Interfaces.Repositories;
using WorkoutPlanner.Domain;
using WorkoutPlanner.Infraestructure.Persistence;

namespace WorkoutPlanner.Infraestructure.Repositories;

public class ExcerciseRepository(AppDbContext context) : GenericRepository<Excercise>(context), IExcerciseRepository
{
}
