import { apiClient } from "./apiClient";
import type { Asset, CreateAsset } from "../types/admin";

const route = "/api/1.0/V1Assets";

export const assetsApi = {
    getAll() {
        return apiClient.get<Asset[]>(route);
    },

    create(asset: CreateAsset) {
        return apiClient.post<Asset>(route, asset);
    },
};