export interface CheckIn {
    memberId: number;
    playgroundId: number;
    checkInDate: Date;
    memberLoginName?: string;
    playgroundAddress?: string;
}
