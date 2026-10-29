namespace KeeperApi.Models;

public class Visitor
{
    public int Id { get; set; }
    public int TrustLevel { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<VisitorAction> Actions { get; set; } = new();
}
