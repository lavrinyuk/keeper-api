namespace KeeperApi.Dtos;

public class ActionExecutionResponse
{
    public int VisitorId { get; set; }
    public string Action { get; set; } = string.Empty;
    public int TrustChange { get; set; }
    public int TrustLevel { get; set; }
    public DateTime PerformedAt { get; set; }
}
