export interface Photo {
    id: number;
    url: string;
    publicId: string;
    memberId?: number;
    playgroundId?: number;
    playgroundAddress?: string;
    playgroundLocationStr?: string;
    playgroundLocationId?: number;
    description?: string;
    created?: Date;
}
