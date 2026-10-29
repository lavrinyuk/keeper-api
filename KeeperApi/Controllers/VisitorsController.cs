using KeeperApi.Dtos;
using KeeperApi.Models;
using KeeperApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace KeeperApi.Controllers;

[ApiController]
[Route("api/visitors")]
public class VisitorsController : ControllerBase
{
    private readonly KeeperService _keeperService;

    public VisitorsController(KeeperService keeperService)
    {
        _keeperService = keeperService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateVisitor()
    {
        var visitor = await _keeperService.CreateVisitorAsync();

        return CreatedAtAction(
            nameof(GetVisitor),
            new { id = visitor.Id },
            ToVisitorResponse(visitor));
    }

    [HttpGet]
    public async Task<ActionResult<List<VisitorResponse>>> GetVisitors()
    {
        var visitors = await _keeperService.GetVisitorsAsync();

        return Ok(visitors.Select(ToVisitorResponse).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VisitorResponse>> GetVisitor(int id)
    {
        var visitor = await _keeperService.GetVisitorAsync(id);

        if (visitor == null)
        {
            return NotFound(new { message = "Visitor not found." });
        }

        return Ok(ToVisitorResponse(visitor));
    }

    [HttpPost("{id:int}/actions")]
    public async Task<IActionResult> PerformAction(int id, [FromBody] ActionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Action))
        {
            return BadRequest(new { message = "Action is required." });
        }

        var result = await _keeperService.PerformActionAsync(id, request.Action);

        if (result.Status == ActionStatus.VisitorNotFound)
        {
            return NotFound(new { message = "Visitor not found." });
        }

        if (result.Status == ActionStatus.InvalidAction)
        {
            return BadRequest(new { message = "Invalid action." });
        }

        if (result.Status == ActionStatus.AlreadyPerformed)
        {
            return BadRequest(new { message = "This action has already been performed." });
        }

        var action = result.ActionRecord!;

        return Ok(new ActionExecutionResponse
        {
            VisitorId = result.Visitor!.Id,
            Action = action.Action,
            TrustChange = action.TrustChange,
            TrustLevel = action.TrustLevelAfter,
            PerformedAt = action.PerformedAt
        });
    }

    [HttpGet("{id:int}/access")]
    public async Task<IActionResult> CheckAccess(int id)
    {
        var result = await _keeperService.CheckAccessAsync(id);

        if (result == null)
        {
            return NotFound(new { message = "Visitor not found." });
        }

        return Ok(result);
    }

    private static VisitorResponse ToVisitorResponse(Visitor visitor)
    {
        return new VisitorResponse
        {
            Id = visitor.Id,
            TrustLevel = visitor.TrustLevel,
            CreatedAt = visitor.CreatedAt,
            Actions = visitor.Actions
                .OrderBy(action => action.Id)
                .Select(action => new ActionResponse
                {
                    Action = action.Action,
                    TrustChange = action.TrustChange,
                    TrustLevelAfter = action.TrustLevelAfter,
                    PerformedAt = action.PerformedAt
                })
                .ToList()
        };
    }
}
