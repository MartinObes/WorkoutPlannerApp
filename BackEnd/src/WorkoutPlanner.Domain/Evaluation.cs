namespace WorkoutPlanner.Domain;

public class Evaluation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PlayerId { get; set; }
    public User Player { get; set; } = null!;
    public DateTime Date { get; set; }
    public Guid ExcerciseId { get; set; }
    public Excercise Excercise { get; set; } = null!;
}