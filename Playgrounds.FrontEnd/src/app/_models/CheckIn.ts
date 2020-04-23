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
}

export interface PlaygroundTimeslots {
    playgroundId: number;
    playgroundAddress?: string;
    playgroundPhotoUrl?: string;
    timeslots: Timeslot[];
}