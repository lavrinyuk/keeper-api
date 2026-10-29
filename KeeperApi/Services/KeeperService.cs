using KeeperApi.Data;
using KeeperApi.Models;
using Microsoft.EntityFrameworkCore;

namespace KeeperApi.Services;

public class KeeperService
{
    private readonly AppDbContext _context;

    public KeeperService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Visitor> CreateVisitorAsync()
    {
        var visitor = new Visitor
        {
            TrustLevel = 0,
            CreatedAt = DateTime.UtcNow
        };

        _context.Visitors.Add(visitor);
        await _context.SaveChangesAsync();

        return visitor;
    }

    public async Task<List<Visitor>> GetVisitorsAsync()
    {
        return await _context.Visitors
            .Include(visitor => visitor.Actions)
            .AsNoTracking()
            .OrderBy(visitor => visitor.Id)
            .ToListAsync();
    }

    public async Task<Visitor?> GetVisitorAsync(int id)
    {
        return await _context.Visitors
            .Include(visitor => visitor.Actions)
            .AsNoTracking()
            .FirstOrDefaultAsync(visitor => visitor.Id == id);
    }

    public async Task<ActionExecutionResult> PerformActionAsync(int id, string action)
    {
        var visitor = await _context.Visitors
            .Include(item => item.Actions)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (visitor == null)
        {
            return new ActionExecutionResult { Status = ActionStatus.VisitorNotFound };
        }

        if (!TryGetTrustChange(action, out var trustChange))
        {
            return new ActionExecutionResult { Status = ActionStatus.InvalidAction };
        }

        if (visitor.Actions.Any(item => item.Action == action))
        {
            return new ActionExecutionResult { Status = ActionStatus.AlreadyPerformed };
        }

        var oldTrustLevel = visitor.TrustLevel;
        visitor.TrustLevel = Math.Clamp(oldTrustLevel + trustChange, 0, 100);

        var actionRecord = new VisitorAction
        {
            VisitorId = visitor.Id,
            Action = action,
            TrustChange = visitor.TrustLevel - oldTrustLevel,
            TrustLevelAfter = visitor.TrustLevel,
            PerformedAt = DateTime.UtcNow
        };

        _context.VisitorActions.Add(actionRecord);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return new ActionExecutionResult { Status = ActionStatus.AlreadyPerformed };
        }

        return new ActionExecutionResult
        {
            Status = ActionStatus.Success,
            Visitor = visitor,
            ActionRecord = actionRecord
        };
    }

    public async Task<AccessResult?> CheckAccessAsync(int id)
    {
        var visitor = await _context.Visitors
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id);

        if (visitor == null)
        {
            return null;
        }

        return new AccessResult
        {
            VisitorId = visitor.Id,
            TrustLevel = visitor.TrustLevel,
            HasAccess = visitor.TrustLevel >= 50
        };
    }

    private static bool TryGetTrustChange(string action, out int trustChange)
    {
        switch (action)
        {
            case "Introduce":
                trustChange = 20;
                return true;
            case "OfferHelp":
                trustChange = 30;
                return true;
            case "Threaten":
                trustChange = -20;
                return true;
            default:
                trustChange = 0;
                return false;
        }
    }
}
