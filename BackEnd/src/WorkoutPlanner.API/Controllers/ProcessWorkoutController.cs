using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner.API.Models;
using WorkoutPlanner.Application.Services.WourkoutProcessorService;

namespace WorkoutPlanner.API.Controllers;

[ApiController]
[Route("process-workout")]
public class ProcessWorkoutController(IWorkoutProcessorService workoutProcessorService) : ControllerBase
{
    private readonly IWorkoutProcessorService _workoutProcessorService = workoutProcessorService;

    [HttpPost]
    public async Task<ProcessedWorkoutResponseDto> ProcessWorkout([FromBody] ProcessWorkoutRequestDto request)
    {
        var result =  await _workoutProcessorService.ProcessWorkout(request.WorkoutName, request.Username);
        return new ProcessedWorkoutResponseDto(result);
    }
}