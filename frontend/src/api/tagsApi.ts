import { apiClient } from "./apiClient";
import type { Tag, CreateTag } from "../types/admin";

const route = "/api/1.0/V1Tags";

export const tagsApi = {
    getAll() {
        return apiClient.get<Tag[]>(route);
    },

    create(tag: CreateTag) {
        return apiClient.post<Tag>(route, tag);
    },
};