namespace WorkoutPlanner.Domain;

public class Evaluation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PlayerId { get; set; }
    public User Player { get; set; } = null!;
    public DateTime Date { get; set; }
    public Guid ExcerciseId { get; set; }
    public Excercise Excercise { get; set; } = null!;
    public int Reps { get; set; }  
    public int Weight { get; set; }
    
    public static void Validate(int Reps, int Weight)
    {
        if (Reps <= 0)
        {
            throw new ArgumentException("Reps must be greater than 0");
        }

        if (Weight < 0)
        {
            throw new ArgumentException("Weight must be greater than or equal to 0");
        }
    }
}