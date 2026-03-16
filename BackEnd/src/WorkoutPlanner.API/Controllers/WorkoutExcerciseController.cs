using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner.API.Models;
using WorkoutPlanner.Application.Services.WourkoutProcessorService;
using WorkoutPlanner.Application.WorkoutExcercises;

namespace WorkoutPlanner.API.Controllers;

[ApiController]
[Route("workout-excercises")]
public class WorkoutExcerciseController(IWorkoutExcerciseLogic workoutExcerciseLogic) : ControllerBase
{
    private readonly IWorkoutExcerciseLogic _workoutExcerciseLogic = workoutExcerciseLogic;
    
    [HttpPut]
    public async Task<WorkoutExcerciseResponseDto> Update([FromBody] UpdateWorkoutExcerciseRequestDto request)
    {
        var result = await _workoutExcerciseLogic.updateWorkoutExcercise(request.WorkoutExcerciseId, request.Name,request.WorkoutId,request.ExcerciseId,
        request.Reps, request.Sets, request.LoadType, request.Weight, request.Percentage);
        return new WorkoutExcerciseResponseDto(result);
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteWorkoutExcerciseRequestDto request)
    {
        await _workoutExcerciseLogic.deleteWorkoutExcercise(request.WorkoutExcerciseId);
        return NoContent();
    }
    
    [HttpGet]
    public async Task<WorkoutExcerciseResponseDto> GetById([FromBody] GetWorkoutExcerciseByIdRequestDto request)
    {
        var result = await _workoutExcerciseLogic.getWorkoutExcerciseById(request.WorkoutExcerciseId);
        return new WorkoutExcerciseResponseDto(result);
    }
    
    [HttpGet]
    public async Task<WorkoutExcercisesResponseDto> GetAll()
    {
        var result = await _workoutExcerciseLogic.getAllWorkoutExcercises();
        return new WorkoutExcercisesResponseDto(result);
    }

}