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

    [HttpDelete("{evaluationId:guid}")]
    public async Task<IActionResult> Delete(Guid evaluationId)
    {
        await _evaluationLogic.DeleteEvaluation(evaluationId);
        return NoContent();
    }

    [HttpGet("{evaluationId:guid}")]
    public async Task<EvaluationResponseDto> GetById(Guid evaluationId)
    {
        var result = await _evaluationLogic.GetEvaluationById(evaluationId);
        return new EvaluationResponseDto(result);
    }

    [HttpGet("by-player/{playerId:guid}")]
    public async Task<EvaluationsResponseDto> GetAllByPlayerId(Guid playerId)
    {
        var result = await _evaluationLogic.GetEvaluationsByPlayerId(playerId);
        return new EvaluationsResponseDto(result);
    }

    [HttpGet("by-exercise/{excerciseId:guid}")]
    public async Task<EvaluationsResponseDto> GetAllByExcerciseId(Guid excerciseId)
    {
        var result = await _evaluationLogic.GetEvaluationsByExcerciseId(excerciseId);
        return new EvaluationsResponseDto(result);
    }

    [HttpGet("compare")]
    public async Task<CompareEvaluationsResponseDto> CompareEvaluations([FromQuery] Guid evaluationId1, [FromQuery] Guid evaluationId2)
    {
        var result = await _evaluationLogic.CompareEvaluations(evaluationId1, evaluationId2);
        return new CompareEvaluationsResponseDto(result);
    }

}