import { Component, OnInit, Input } from '@angular/core';
import { CheckInToDisplay, CheckinsTimeslots } from 'src/app/_models/CheckIn';

@Component({
  selector: 'app-upcoming-checkins',
  templateUrl: './upcoming-checkins.component.html',
  styleUrls: ['./upcoming-checkins.component.scss']
})
export class UpcomingCheckinsComponent implements OnInit {
  constructor() { }
  
  @Input() playgroundId: number;
  @Input() isLoggedIn: boolean;
  @Input() userName?: string;
  @Input() userPhotoUrl?: string;
  @Input() todaysCheckinsTimeSlots: CheckinsTimeslots[];
  @Input() endLastTimeslot: string;

  ngOnInit() {
    console.log(this.todaysCheckinsTimeSlots.length);
    // this.getTodaysCheckinTimeslots();
  }
}
