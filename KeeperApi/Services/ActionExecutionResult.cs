using KeeperApi.Models;

namespace KeeperApi.Services;

public class ActionExecutionResult
{
    public ActionStatus Status { get; set; }
    public Visitor? Visitor { get; set; }
    public VisitorAction? ActionRecord { get; set; }
}
