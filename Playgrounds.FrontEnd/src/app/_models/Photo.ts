export interface Photo {
    id: number;
    url: string;
    publicId: string;
    memberId?: number;
    memberLoginName?: string;
    playgroundId?: number;
    playgroundAddress?: string;
    playgroundLocationStr?: string;
    playgroundLocationId?: number;
    description?: string;
    created?: Date;
}

export interface PhotoToUpload {
    playgroundId?: number;
    description?: string;
}
