using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner.API.Models;
using WorkoutPlanner.Application.Workouts;

namespace WorkoutPlanner.API.Controllers;

[ApiController]
[Route("workouts")]
public class WorkoutController(IWorkoutLogic workoutLogic) : ControllerBase
{
    private readonly IWorkoutLogic _workoutLogic = workoutLogic;

    [HttpGet]
    public async Task <WorkoutsResponseDto> GetAll()
    {
        var result = await _workoutLogic.GetAllWorkouts();
        return new WorkoutsResponseDto(result);
    }

    [HttpGet]
    public async Task<WorkoutResponseDto> GetByName([FromBody] GetWorkoutByNameRequestDto request)
    {
        var result = await _workoutLogic.GetWorkoutByName(request.Name);
        return new WorkoutResponseDto(result);
    }

    [HttpPost]
     public async Task<WorkoutResponseDto> Create([FromBody] CreateWorkoutRequestDto request)
     {
         var result = await _workoutLogic.CreateWorkout(request.Name, request.CoachId, request.WorkoutExcercises);
         return new WorkoutResponseDto(result);
     }

     [HttpDelete]
     public async Task<IActionResult> Delete([FromBody] DeleteWorkoutRequestDto request)
     {
         await _workoutLogic.DeleteWorkout(request.Name);
         return NoContent();
     }

     [HttpPut]
     public async Task<WorkoutResponseDto> Update([FromBody] UpdateWorkoutRequestDto request)
     {
         var result = await _workoutLogic.UpdateWorkout(request.WorkoutId, request.Name, request.CoachId);
         return new WorkoutResponseDto(result);
     }

}