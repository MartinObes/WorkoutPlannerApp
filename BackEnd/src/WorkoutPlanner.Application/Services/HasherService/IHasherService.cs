namespace WorkoutPlanner.Application.Services.HasherService;

public interface IHasherService
{
    string Hash(string password);
    bool Verify(string hashedPassword, string providedPassword);
}