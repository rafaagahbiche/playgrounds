// CheckinForCreationDto
export interface CheckIn {
    playgroundId: number;
    checkInDate: Date;
}

// CheckinDto
export interface CheckInToDisplay {
    id: number;
    checkInDate: Date;
    memberLoginName?: string;
    memberProfilePictureUrl?: string;
    playgroundAddress?: string;
}

export interface CheckinsTimeslots {
    checkins: CheckInToDisplay[];
    startsAt: Date;
    selected?: boolean;
}
