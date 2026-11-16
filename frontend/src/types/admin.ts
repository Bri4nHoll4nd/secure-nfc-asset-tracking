export interface Asset {
    id: number,
    name: string,
    description: string,
    status: string,
    maintenanceStatus: string,
    userId: number | null;
}

export interface CreateAsset {
    name: string,
    description: string,
    status: string,
    maintenanceStatus: string,
    userId: number | null;
}

export interface User {
    id: number,
    name: string,
    status: string;
}

export interface CreateUser {
    name: string,
    status: string;
}

export interface Tag {
    id: number,
    name: string,
    version: string,
    signature: number[],
    assetId: number | null,
    userId: number | null;
}

export interface CreateTag {
    name: string,
    version: string,
    signature: number[],
    assetId: number | null,
    userId: number | null;
}