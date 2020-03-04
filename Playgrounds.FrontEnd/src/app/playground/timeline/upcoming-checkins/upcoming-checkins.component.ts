import { Component, OnInit, Input } from '@angular/core';
import { CheckinsService } from 'src/app/_services/checkins.service';
import { CheckIn, CheckInToDisplay, CheckinsTimeslots } from 'src/app/_models/CheckIn';

@Component({
  selector: 'app-upcoming-checkins',
  templateUrl: './upcoming-checkins.component.html',
  styleUrls: ['./upcoming-checkins.component.scss']
})
export class UpcomingCheckinsComponent implements OnInit {
  constructor(private checkinsService: CheckinsService) { }
  @Input() playgroundId: number;
  @Input() isLoggedIn: boolean;
  @Input() userName?: string;
  @Input() userPhotoUrl?: string;

  checkins: CheckInToDisplay[];
  startingTime: string;
  countMoreCheckins = 0;
  todaysCheckinsTimeSlots: CheckinsTimeslots[];
  endLastTimeslot: string;
  
  ngOnInit() {
    this.getTodaysCheckinTimeslots();
  }

  private getTodaysCheckins() {
    this.checkinsService.getCheckInsAtPlaygroundByDate(this.playgroundId, new Date()).subscribe((checkins: CheckInToDisplay[]) => {
      if (checkins !== undefined && checkins !== null) {
        this.startingTime = checkins[0].checkInDate.getHours + ':' + checkins[0].checkInDate.getHours;
        if (checkins.length > 3){
          this.checkins = [checkins[0],checkins[1],checkins[2]];
          this.countMoreCheckins = checkins.length - 3;
        } else {
          this.checkins = checkins;
        }
      }
    });
  }

  private getTodaysCheckinTimeslots() {
    var todaysDate = new Date();
    var todaysDateStr = todaysDate.getFullYear() + '-' + (1 + todaysDate.getMonth()) + '-' + todaysDate.getDate();
    this.checkinsService.getCheckinsSlotsAtPlaygroundByDate(this.playgroundId, todaysDateStr).subscribe((checkinsTimeSlots: CheckinsTimeslots[]) => {
      if (checkinsTimeSlots !== undefined && checkinsTimeSlots !== null && checkinsTimeSlots.length > 0) {
        this.todaysCheckinsTimeSlots = checkinsTimeSlots;
        this.setEndLastTimeslot();
      }
    });
  }

  private setEndLastTimeslot() {
    if (this.todaysCheckinsTimeSlots.length > 0) {
      var count = this.todaysCheckinsTimeSlots.length;
      var lastSlot = this.todaysCheckinsTimeSlots[count -1];
      var lastStart = new Date(lastSlot.startsAt);
      this.endLastTimeslot = ('0' + (lastStart.getHours() + 2)).slice(-2) + ':' + ('0' +lastStart.getMinutes()).slice(-2);
    }
  }
}
