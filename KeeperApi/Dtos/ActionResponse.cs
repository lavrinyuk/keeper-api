namespace KeeperApi.Dtos;

public class ActionResponse
{
    public string Action { get; set; } = string.Empty;
    public int TrustChange { get; set; }
    public int TrustLevelAfter { get; set; }
    public DateTime PerformedAt { get; set; }
}
