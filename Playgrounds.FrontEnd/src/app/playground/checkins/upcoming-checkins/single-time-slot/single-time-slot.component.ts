import { Component, OnInit, Input } from '@angular/core';
import { CheckInToDisplay, CheckIn } from 'src/app/_models/CheckIn';
import { AuthService } from 'src/app/_services/auth.service';
import { CheckinsService } from 'src/app/_services/checkins.service';

@Component({
  selector: 'app-single-time-slot',
  templateUrl: './single-time-slot.component.html',
  styleUrls: ['./single-time-slot.component.scss']
})
export class SingleTimeSlotComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private checkinsService: CheckinsService) { }

  @Input() participants: CheckInToDisplay[];
  @Input() startsAt: Date;
  @Input() playgroundId: number;
  @Input() isLoggedIn: boolean;
  @Input() userName?: string;
  @Input() userPhotoUrl?: string;

  startsAtStr: string;
  moreHoopersPlaying: string;
  loggedInMemberIsAmongParticipants: boolean;
  
  ngOnInit() {
    this.setTimeSlot();
    this.setMoreHoopersPlaying();
    this.setThreeParticipants();
    this.isLoggedInMemberAmongParticipants();
    this.reverseListOfParticipants();
  }

  private setTimeSlot() {
    var timeSlotStartsAt = new Date(this.startsAt);
    this.startsAtStr = ('0' + timeSlotStartsAt.getHours()).slice(-2) + ':' + ('0' + timeSlotStartsAt.getMinutes()).slice(-2);
  }

  private setMoreHoopersPlaying() {
    if (this.participants.length > 3) {
      this.moreHoopersPlaying = '+' + (this.participants.length - 3) + ' more Hoopers are playing';
    }
  }

  private reverseListOfParticipants() {
    this.participants = this.participants.reverse();
    if (this.loggedInMemberIsAmongParticipants) {
      var indexOfMember = this.participants.findIndex(p => p.memberLoginName === this.userName);
      var memberCheckin = this.participants.splice(indexOfMember, 1);
      this.participants.unshift(memberCheckin[0]);
    }
  }

  private setThreeParticipants() {
    if (this.participants.length > 3) {
      this.participants.length = 3;
    }
  }

  private isLoggedInMemberAmongParticipants() {
    this.loggedInMemberIsAmongParticipants = false;
    if (this.isLoggedIn) {
      var indexOfMember = this.participants.findIndex(p => p.memberLoginName === this.userName);
      this.loggedInMemberIsAmongParticipants = indexOfMember > -1;
    }
  }

  checkinMemberToPlayground() {
    const checkinModel: CheckIn = {
      checkInDate: this.startsAt,
      playgroundId : this.playgroundId
    };
    this.checkinsService.checkInToPlayground(checkinModel, this.authService.getMemberToken())
      .subscribe((checkinToDisplay: CheckInToDisplay) => {
        if (checkinToDisplay !== null && checkinToDisplay !== undefined) {
          this.loggedInMemberIsAmongParticipants = true;
          this.participants.unshift(checkinToDisplay);
          this.setMoreHoopersPlaying();
          this.setThreeParticipants();
        }
    });
  }

  cancelCheckinMember() {
    var checkinMember = this.participants.find(p => p.memberLoginName === this.userName);
    this.checkinsService.cancelCheckin(checkinMember.id, this.authService.getMemberToken())
      .subscribe(next => {
          // this.participants.unshift(checkInViewModel);
          this.loggedInMemberIsAmongParticipants = false;
          this.participants.shift();
          this.setMoreHoopersPlaying();
          this.setThreeParticipants();
    });
  }
}
