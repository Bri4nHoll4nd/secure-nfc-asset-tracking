namespace SecureNfc.Data.Models.V1;

public class V1User
{
    public int Id { get; set; }
    public required string EntityCode { get; set; }
    public required string Name { get; set; }
    public required string Status { get; set; }

    public V1Tag? Tag { get; set; }

    public List<V1Asset> Assets { get; set; } = [];
    public List<V1Log> AssetLogs { get; set; } = [];
}