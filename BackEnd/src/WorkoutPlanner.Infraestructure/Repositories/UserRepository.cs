using WorkoutPlanner.Application.Interfaces.Repositories;
using WorkoutPlanner.Domain;
using WorkoutPlanner.Infraestructure.Persistence;

namespace WorkoutPlanner.Infraestructure.Repositories;

public class UserRepository(AppDbContext context) : GenericRepository<User>(context), IUserRepository
{
}