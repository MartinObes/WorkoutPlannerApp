namespace WorkoutPlanner.Domain;

public class Excercise
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
}