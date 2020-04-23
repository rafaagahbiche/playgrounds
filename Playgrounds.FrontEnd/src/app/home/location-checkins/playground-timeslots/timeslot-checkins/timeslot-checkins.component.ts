import { Component, OnInit, Input } from '@angular/core';
import { CheckInToDisplay } from 'src/app/_models/CheckIn';

@Component({
  selector: 'app-timeslot-checkins',
  templateUrl: './timeslot-checkins.component.html',
  styleUrls: ['./timeslot-checkins.component.scss']
})
export class HomePlaygroundTimeslotCheckinsComponent implements OnInit {

  constructor() { }
  @Input() checkins : CheckInToDisplay[] = new Array()
  ngOnInit() {
    if (this.checkins.length > 3) {
      this.checkins.length = 3;
    }
  }

}
