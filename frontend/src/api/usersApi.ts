import { apiClient } from "./apiClient";
import type { User, CreateUser } from "../types/admin";

const route = "/api/1.0/V1Users";

export const usersApi = {
    getAll() {
        return apiClient.get<User[]>(route);
    },

    create(user: CreateUser) {
        return apiClient.post<User>(route, user);
    },
};