import { Component, OnInit, Input } from '@angular/core';
import { CheckInToDisplay, CheckIn } from 'src/app/_models/CheckIn';
import { AuthService } from 'src/app/_services/auth.service';
import { CheckinsService } from 'src/app/_services/checkins.service';
import { fadeInAnimation } from 'src/app/_animations/fadeInAnimation';
import { TimeslotSelectionService } from 'src/app/_services/timeslot-selection.service';

@Component({
  selector: 'app-single-timeslot',
  templateUrl: './single-timeslot.component.html',
  styleUrls: ['./single-timeslot.component.scss'],
  animations: [fadeInAnimation]
})
export class SingleTimeslotComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private checkinsService: CheckinsService,
    private timeslotSelectionService: TimeslotSelectionService) { }

  @Input() participants: CheckInToDisplay[];
  @Input() startsAt: Date;
  @Input() playgroundId: number;
  @Input() isLoggedIn: boolean;
  @Input() userName?: string;
  @Input() userPhotoUrl?: string;

  showTimeslotDetails: boolean;
  moreHoopersPlaying: string;
  loggedInMemberIsAmongParticipants: boolean;
  pixelsToTranslate: string;
  
  ngOnInit() {
    this.timeslotSelectionService.currentTimeslotSelected.subscribe(timeslotSelected => {
      this.showTimeslotDetails = timeslotSelected !== undefined 
                              && timeslotSelected !== null
                              && (new Date(this.startsAt).getTime()).toString() === timeslotSelected;
    });

    this.timeslotSelectionService.currentPixelsToTranslate.subscribe(pixelsToTranslate => {
      this.pixelsToTranslate = pixelsToTranslate + 'px';
    });

    this.reverseListOfParticipants();
    // this.setMoreHoopersPlaying();
    // this.setThreeParticipants();
    this.isLoggedInMemberAmongParticipants();
  }

  private setMoreHoopersPlaying() {
    if (this.participants.length == 6) {
      this.moreHoopersPlaying = '+1 more Hooper.';
    }
    if (this.participants.length > 6) {
      this.moreHoopersPlaying = '+' + (this.participants.length - 5) + ' more Hoopers.';
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
    if (this.participants.length > 5) {
      this.participants.length = 5;
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
          // this.setMoreHoopersPlaying();
          // this.setThreeParticipants();
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
          // this.setMoreHoopersPlaying();
          // this.setThreeParticipants();
    });
  }
}
