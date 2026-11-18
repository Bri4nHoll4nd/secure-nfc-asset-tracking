public enum RegistrationStatus
{
    WaitingForTag,
    TagAvailable,
    WaitingForWrite,
    Writing,
    Completed,
    Failed,
    Cancelled
}

public class V1AssetRegistration
{
    public Guid Id {  get; set; } = Guid.NewGuid();

    public string ReaderId { get; set; } = "reader-01";

    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Status { get; set; } = "";
    public string MaintenanceStatus { get; set; } = "";

    public string? Uid { get; set; }
    public string Version { get; set; } = "1.0";
    public string? SignatureHex { get; set; }

    public RegistrationStatus RegistrationStatus { get; set; }
        = RegistrationStatus.WaitingForTag;

    public string? Error { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
}