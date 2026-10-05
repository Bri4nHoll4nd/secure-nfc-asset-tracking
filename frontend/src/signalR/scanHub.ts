import {
    HubConnectionBuilder,
    LogLevel
} from "@microsoft/signalr";

import { API_BASE_URL } from "../api/apiClient";

export function createScanHubConnection() {
    return new HubConnectionBuilder()
        .withUrl(`${API_BASE_URL}/hubs/scans`)
        .withAutomaticReconnect()
        .configureLogging(LogLevel.Information)
        .build();
}