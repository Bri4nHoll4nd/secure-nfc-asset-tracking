namespace SecureNfc.Data.Models.V1;

public enum AssetLogType
{
    Created = 0,
    CheckedOut = 1,
    CheckedIn = 2,
    MaintenanceStarted = 3,
    MaintenanceCompleted = 4,
    Updated = 5,
    TagReplaced = 6
}