using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner.API.Models;
using WorkoutPlanner.Application.Evaluations;

namespace WorkoutPlanner.API.Controllers;

[ApiController]
[Route("evaluations")]
public class EvaluationController(IEvaluationLogic evaluationLogic) : ControllerBase
{
    private readonly IEvaluationLogic _evaluationLogic = evaluationLogic;

    [HttpPost]
    public async Task<EvaluationResponseDto> Create([FromBody] CreateEvaluationRequestDto request)
    {
        var result = await _evaluationLogic.CreateEvaluation(request.PlayerId, request.ExcerciseId, request.Reps, request.Weight);
        return new EvaluationResponseDto(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteEvaluationRequestDto request)
    {
        await _evaluationLogic.DeleteEvaluation(request.EvaluationId);
        return NoContent();
    }

    [HttpGet("/{evaluationId}")]
    public async Task<EvaluationResponseDto> GetById(Guid evaluationId)
    {
        var result = await _evaluationLogic.GetEvaluationById(evaluationId);
        return new EvaluationResponseDto(result);
    }

    [HttpGet]
    public async Task<EvaluationsResponseDto> GetAllByPlayerId([FromBody] GetEvaluationsByPlayerIdRequestDto request)
    {
        var result = await _evaluationLogic.GetEvaluationsByPlayerId(request.PlayerId);
        return new EvaluationsResponseDto(result);
    }
    
    [HttpGet]
    public async Task<EvaluationsResponseDto> GetAllByExcerciseId([FromBody] GetEvaluationsByExcerciseIdRequestDto request)
    {
        var result = await _evaluationLogic.GetEvaluationsByExcerciseId(request.ExcerciseId);
        return new EvaluationsResponseDto(result);
    }

    [HttpGet]
    public async Task<CompareEvaluationsResponseDto> CompareEvaluations([FromBody] CompareEvaluationsRequestDto request)
    {
        var result = await _evaluationLogic.CompareEvaluations(request.EvaluationId1, request.EvaluationId2);
        return new CompareEvaluationsResponseDto(result);
    }

}