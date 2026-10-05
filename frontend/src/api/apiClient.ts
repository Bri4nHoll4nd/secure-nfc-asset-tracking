export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? "http://localhost:5201";

export interface ScanResult {
    tagUid: string;
    source: string;
    scannedAtUtc: string;

    registered: boolean;

    tagId: number | null;

    entityType:
        | "Asset"
        | "User"
        | "Unassigned"
        | "Unknown";

    entityId: number | null;

    name: string | null;

    status: string | null;
}

export async function getApiStatus(): Promise<string> {
    const response = await fetch(`${API_BASE_URL}/api/1.0/V1Status`);

    if (!response.ok) {
        throw new Error(`API request failed: ${response.status}`);
    }

    return response.text();
}

export async function getLatestScan(): Promise<ScanResult | null> {
    const response = await fetch(`${API_BASE_URL}/api/1.0/V1Scans/latest`);

    if (response.status === 204) {
        return null;
    }

    if (!response.ok) {
        throw new Error(
            `API request failed: ${response.status}`
        );
    }

    return response.json();
}