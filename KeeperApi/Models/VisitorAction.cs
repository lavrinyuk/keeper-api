namespace KeeperApi.Models;

public class VisitorAction
{
    public int Id { get; set; }
    public int VisitorId { get; set; }
    public Visitor Visitor { get; set; } = null!;
    public string Action { get; set; } = string.Empty;
    public int TrustChange { get; set; }
    public int TrustLevelAfter { get; set; }
    public DateTime PerformedAt { get; set; }
}
