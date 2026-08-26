namespace SecureNfc.Data.Models.V1;

public class V1User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }

    public List<V1Asset> Assets { get; set; } = [];
}