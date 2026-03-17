using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner.API.Models;
using WorkoutPlanner.Application.Excercises;

namespace WorkoutPlanner.API.Controllers;

[ApiController]
[Route("exercises")]
public class ExerciseController(IExcerciseLogic excerciseLogic) : ControllerBase
{
    private readonly IExcerciseLogic _excerciseLogic = excerciseLogic;

    //GET/atributes
    [HttpGet]
    public async Task<GetAllExcercisesResponseDto> GetAll()
    {
        var result = await _excerciseLogic.GetAllExcercises();
        return new GetAllExcercisesResponseDto(result);
    }

    //POST/atributes
    [HttpPost]
    public async Task<ExcerciseResponseDto> Create([FromBody] CreateExcerciseRequestDto request)
    {
        var result = await _excerciseLogic.CreateExcercise(request.Name);
        return new ExcerciseResponseDto(result);
    }

    //DELETE/atributes/{id}
    [HttpDelete("{excerciseName}")]
    public async Task<IActionResult> Delete(string excerciseName)
    {
        await _excerciseLogic.DeleteExcercise(excerciseName);
        return NoContent();
    }

    //GET/atributes/{name}
    [HttpGet("{excerciseName}")]
    public async Task<ExcerciseResponseDto> GetByName(string excerciseName)
    {
        var result = await _excerciseLogic.GetExcerciseByName(excerciseName);
        return new ExcerciseResponseDto(result);
    }

}