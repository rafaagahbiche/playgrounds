export interface Photo {
    id: number;
    url: string;
    publicId: string;
    memberId?: number;
    playgroundId?: number;
    description?: string;
    created?: Date;
}
