namespace KeeperApi.Dtos;

public class VisitorResponse
{
    public int Id { get; set; }
    public int TrustLevel { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ActionResponse> Actions { get; set; } = new();
}
