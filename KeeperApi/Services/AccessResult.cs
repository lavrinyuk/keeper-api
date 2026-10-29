namespace KeeperApi.Services;

public class AccessResult
{
    public int VisitorId { get; set; }
    public int TrustLevel { get; set; }
    public bool HasAccess { get; set; }
}
