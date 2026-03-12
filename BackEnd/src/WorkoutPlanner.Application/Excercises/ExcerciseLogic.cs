using WorkoutPlanner.Application.Interfaces.Repositories;
using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.Excercises;

public class ExcerciseLogic (IExcerciseRepository excerciseRepository) : IExcerciseLogic
{
    private const string SpecialCharacters = "!@#$%^&*()_+[]{}|;:',.<>?/`~-=";
    private readonly IExcerciseRepository _excerciseRepository = excerciseRepository  ?? throw new ArgumentNullException(nameof(excerciseRepository));
    

    public async Task<Excercise> CreateExcercise(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Excercise name cannot be empty."); 
        if (name.Any(ch => SpecialCharacters.Contains(ch)))
                throw new ArgumentException("Excercise name cannot contain special characters.");
            
        var excercise =  new Excercise 
        {
            Id = Guid.NewGuid(),
            Name = name.Trim()
        };
        
        await _excerciseRepository.InsertAsync(excercise);
        return excercise;
    }
    
    public void DeleteExcercise(Excercise excercise)
    {
        if (excercise == null) throw new ArgumentNullException(nameof(excercise), "Excercise cannot be null.");

        _excerciseRepository.Delete(excercise);
    }
}