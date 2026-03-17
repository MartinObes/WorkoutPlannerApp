using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner.API.Models;
using WorkoutPlanner.Application.WorkoutExcercises;

namespace WorkoutPlanner.API.Controllers;

[ApiController]
[Route("workout-excercises")]
public class WorkoutExcerciseController(IWorkoutExcerciseLogic workoutExcerciseLogic) : ControllerBase
{
    private readonly IWorkoutExcerciseLogic _workoutExcerciseLogic = workoutExcerciseLogic;

    [HttpPut("{workoutExcerciseId:guid}")]
    public async Task<WorkoutExcerciseResponseDto> Update(Guid workoutExcerciseId, [FromBody] UpdateWorkoutExcerciseRequestDto request)
    {
        var result = await _workoutExcerciseLogic.updateWorkoutExcercise(workoutExcerciseId, request.Name, request.WorkoutId, request.ExcerciseId,
        request.Reps, request.Sets, request.LoadType, request.Weight, request.Percentage);
        return new WorkoutExcerciseResponseDto(result);
    }

    [HttpDelete("{workoutExcerciseId:guid}")]
    public async Task<IActionResult> Delete(Guid workoutExcerciseId)
    {
        await _workoutExcerciseLogic.deleteWorkoutExcercise(workoutExcerciseId);
        return NoContent();
    }

    [HttpGet("{workoutExcerciseId:guid}")]
    public async Task<WorkoutExcerciseResponseDto> GetById(Guid workoutExcerciseId)
    {
        var result = await _workoutExcerciseLogic.getWorkoutExcerciseById(workoutExcerciseId);
        return new WorkoutExcerciseResponseDto(result);
    }

    [HttpGet]
    public async Task<WorkoutExcercisesResponseDto> GetAll()
    {
        var result = await _workoutExcerciseLogic.getAllWorkoutExcercises();
        return new WorkoutExcercisesResponseDto(result);
    }

}