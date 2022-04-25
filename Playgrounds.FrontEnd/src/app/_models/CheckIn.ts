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
}

export interface Timeslot {
    startsAt: Date;
    checkins: CheckInToDisplay[];
    playgroundId?: number;
    playgroundAddress?: string;
}

export interface PlaygroundTimeslots {
    playgroundId: number;
    timeslots: Timeslot[];
    playgroundAddress?: string;
    playgroundPhotoUrl?: string;
}