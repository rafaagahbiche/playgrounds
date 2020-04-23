import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { CheckinsService } from 'src/app/_services/checkins.service';
import { Timeslot } from 'src/app/_models/CheckIn';
import { TimeslotSelectionService } from 'src/app/_services/timeslot-selection.service';

@Component({
  selector: 'app-checkins',
  templateUrl: './checkins.component.html',
  styleUrls: ['./checkins.component.scss']
})

export class PlaygroundCheckinsComponent implements OnInit {
  userName: string;
  userPhotoUrl: string;
  playgroundId: number;
  isLoggedIn: boolean;
  todaysCheckinsTimeSlots: Timeslot[] = new Array<Timeslot>();
  showCheckinForm: boolean;
  showBottomMessage: boolean = false;
  showTopMessage: boolean = false;

  constructor(
    private authService: AuthService,
    private checkinsService: CheckinsService,
    private route: ActivatedRoute,
    private spinner: NgxSpinnerService,
    private timeslotSelectionService: TimeslotSelectionService) { }

  ngOnInit() {
    this.playgroundId = this.route.snapshot.parent.params.id;
    this.authService.currentMemberName.subscribe(name => this.userName = name);
    this.authService.currentMemberPhotoUrl.subscribe(photoUrl => this.userPhotoUrl = photoUrl);
    this.authService.currentLoggedInStatus.subscribe(isLoggedIn => this.isLoggedIn = isLoggedIn);
    this.getTodaysCheckinTimeslots();
    this.showCheckinForm = false;
    
  }

  private getTodaysCheckinTimeslots() {
    var todaysDate = new Date();
    var todaysDateStr = todaysDate.getFullYear() + '-' + (1 + todaysDate.getMonth()) + '-' + todaysDate.getDate();
    this.spinner.show('checkins-spinner');
    setTimeout(() => {
      this.checkinsService.getCheckinsSlotsAtPlaygroundByDate(this.playgroundId, todaysDateStr).subscribe((checkinsTimeSlots: Timeslot[]) => {
        if (checkinsTimeSlots !== undefined && checkinsTimeSlots !== null) {
          this.todaysCheckinsTimeSlots = checkinsTimeSlots;
          this.timeslotSelectionService.setTimeslotSelection((new Date(this.todaysCheckinsTimeSlots[0].startsAt)).getTime().toString());
          this.showBottomMessage = this.todaysCheckinsTimeSlots.length > 0 && this.isLoggedIn;
          this.showTopMessage = this.isLoggedIn && this.todaysCheckinsTimeSlots.length === 0;
        } else {
          this.showTopMessage = this.isLoggedIn;
        }
        this.spinner.hide('checkins-spinner');
      });
    }, 3000);
  }

  openCheckinForm() {
    this.showCheckinForm = true;
  }

  disposeCheckinForm() {
    this.showCheckinForm = false;
  }
}
