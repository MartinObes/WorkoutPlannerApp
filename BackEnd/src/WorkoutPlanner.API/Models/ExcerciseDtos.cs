using WorkoutPlanner.Domain;

namespace WorkoutPlanner.API.Models;

public class CreateExcerciseRequestDto
{
    public string Name { get; set; } = string.Empty;
}

public class ExcerciseResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ExcerciseResponseDto()
    {
    }

    public ExcerciseResponseDto(Excercise excercise)
    {
        Id = excercise.Id;
        Name = excercise.Name;
    }
}

public class GetAllExcercisesResponseDto
{
    public IList<ExcerciseResponseDto> Excercises { get; set; } = new List<ExcerciseResponseDto>();

    public GetAllExcercisesResponseDto()
    {
    }

    public GetAllExcercisesResponseDto(IList<Excercise> excercises)
    {
        Excercises = excercises.Select(excercise => new ExcerciseResponseDto(excercise)).ToList();
    }
}
