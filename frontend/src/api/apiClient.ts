import { matchRoutes } from "react-router-dom";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? "http://localhost:5201";

async function request<T>(
    endpoint: string,
    options?: RequestInit
): Promise<T> {
    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
        ...options,
        headers: {
            "Content-Type": "application/json",
            ...options?.headers,
        },
    });

    const text = await response.text();

    if (!response.ok) {
        let message = `API error ${response.status}`;

        if (text) {
            try {
                const data = JSON.parse(text);
                message =
                    data.message ??
                    data.title ??
                    data.detail ??
                    text;
            } catch {
                message = text;
            }
        }

        throw new Error(message);
    }

    if (!text) {
        return undefined as T;
    }

    return JSON.parse(text) as T;
}

export const apiClient = {
    get<T>(endpoint: string) {
        return request<T>(endpoint);
    },

    post<T>(endpoint: string, body: unknown) {
        return request<T>(endpoint, {
            method: "POST",
            body: JSON.stringify(body),
        });
    },

    put<T>(endpoint: string, body: unknown) {
        return request<T>(endpoint, {
            method: "PUT",
            body: JSON.stringify(body),
        });
    },

    delete<T>(endpoint: string) {
        return request<T>(endpoint, {
            method: "DELETE",
        });
    },
};