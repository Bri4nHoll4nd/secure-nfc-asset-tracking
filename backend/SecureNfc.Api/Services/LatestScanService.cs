using SecureNfc.Api.DTOs;

namespace SecureNfc.Api.Services;

public class LatestScanService
{
    private readonly object _lock = new();
    private V1ScanResponse? _latestScan;

    public void SetLatest(V1ScanResponse scan)
    {
        lock (_lock)
        {
            _latestScan = scan;
        }
    }

    public V1ScanResponse? GetLatest()
    {
        lock (_lock)
        {
            return _latestScan;
        }
    }
}