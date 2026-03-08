namespace WorkoutPlanner.Domain;

public class Workout
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public Guid? CoachId { get; set; }
    public User? Coach { get; set; } = null!;
    public List<WorkoutExcercise> WorkoutExcercises { get; set; } = new List<WorkoutExcercise>();
    
}