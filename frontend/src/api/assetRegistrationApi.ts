import { apiClient } from "./apiClient";

export type RegistrationStatus =
    | "WaitingForTag"
    | "TagAvailable"
    | "WaitingForWrite"
    | "Writing"
    | "Completed"
    | "Failed"
    | "Cancelled";

export interface CreateAssetRegistration {
    name: string;
    description: string;
    status: string;
    maintenanceStatus: string;
    readerId: string;
}

export interface AssetRegistration {
    id: string;
    readerId: string;
    uid: string | null;
    registrationStatus: RegistrationStatus;
    error: string | null;
}

const endpoint = "/api/asset-registrations";

export const assetRegistrationApi = {
    start(data: CreateAssetRegistration) {
        return apiClient.post<AssetRegistration>(
            endpoint,
            data
        );
    },

    get(id: string) {
        return apiClient.get<AssetRegistration>(
            `${endpoint}/${id}`
        );
    },

    write(id: string) {
        return apiClient.post<AssetRegistration>(
            `${endpoint}/${id}/write`,
            {}
        );
    },

    cancel(id: string) {
        return apiClient.delete<void>(
            `${endpoint}/${id}`
        );
    },
};