import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { CheckinsService } from 'src/app/_services/checkins.service';
import { CheckinsTimeslots } from 'src/app/_models/CheckIn';

@Component({
  selector: 'app-checkins',
  templateUrl: './checkins.component.html',
  styleUrls: ['./checkins.component.scss']
})

export class CheckinsComponent implements OnInit {
  userName: string;
  userPhotoUrl: string;
  playgroundId: number;
  isLoggedIn: boolean;
  todaysCheckinsTimeSlots: CheckinsTimeslots[];
  endLastTimeslot: string;
  showCheckinForm: boolean;

  constructor(
    private authService: AuthService,
    private checkinsService: CheckinsService,
    private route: ActivatedRoute,
    private spinner: NgxSpinnerService) { }

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
      this.checkinsService.getCheckinsSlotsAtPlaygroundByDate(this.playgroundId, todaysDateStr).subscribe((checkinsTimeSlots: CheckinsTimeslots[]) => {
        if (checkinsTimeSlots !== undefined && checkinsTimeSlots !== null && checkinsTimeSlots.length > 0) {
          this.todaysCheckinsTimeSlots = checkinsTimeSlots;
          this.setEndLastTimeslot();
        }
        this.spinner.hide('checkins-spinner');
      });
    }, 3000);
  }

  private setEndLastTimeslot() {
    if (this.todaysCheckinsTimeSlots.length > 0) {
      var count = this.todaysCheckinsTimeSlots.length;
      var lastSlot = this.todaysCheckinsTimeSlots[count -1];
      var lastStart = new Date(lastSlot.startsAt);
      this.endLastTimeslot = ('0' + (lastStart.getHours() + 2)).slice(-2) + ':' + ('0' +lastStart.getMinutes()).slice(-2);
    }
  }

  openCheckinForm() {
    this.showCheckinForm = true;
  }

  disposeCheckinForm() {
    this.showCheckinForm = false;
  }
}
