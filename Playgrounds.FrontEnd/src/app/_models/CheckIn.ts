export interface CheckIn {
    memberId: number;
    playgroundId: number;
    checkInDate: Date;
    memberLoginName?: string;
    playgroundAddress?: string;
}

export interface CheckInToDisplay {
    id: number;
    checkInDate: Date;
    memberLoginName?: string;
    memberProfilePictureUrl?: string;
    playgroundAddress?: string;
}
