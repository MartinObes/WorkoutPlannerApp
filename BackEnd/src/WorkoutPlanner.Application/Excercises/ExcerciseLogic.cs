using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.Excercises;

public class ExcerciseLogic : IExcerciseLogic
{
    private string _specialCharacters = "!@#$%^&*()_+[]{}|;:',.<>?/`~-=";

    public Excercise CreateExcercise(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Excercise name cannot be empty."); if (name.Any(ch => _specialCharacters.Contains(ch)))
                throw new ArgumentException("Excercise name cannot contain special characters.");
            
        return new Excercise 
        {
            Id = Guid.NewGuid(),
            Name = name.Trim()
        };
    }
    
    public void DeleteExcercise(Excercise excercise)
    {
        // No specific logic needed for deleting an excercise in this context
        // The actual deletion would be handled by the data access layer or repository
    }
}