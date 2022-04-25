import { Component, OnInit } from '@angular/core';
import { CheckinsService } from 'src/app/_services/checkins.service';
import { Timeslot, PlaygroundTimeslots } from 'src/app/_models/CheckIn';

@Component({
  selector: 'app-location-checkins',
  templateUrl: './location-checkins.component.html',
  styleUrls: ['./location-checkins.component.scss']
})
export class LocationCheckinsComponent implements OnInit {
  locationTimeslots : PlaygroundTimeslots[] = new Array();
  dateValue: Date = new Date();
  dateString: string = "Today";
  showOneDayBeforeButton: boolean = false;
  constructor(private checkinsService: CheckinsService) { }

  ngOnInit() {
    this.setTimeslotsByDate(this.dateValue);
  }

  setTimeslotsByDate(timeslotsDate: Date) {
    var todaysDateStr = timeslotsDate.getFullYear() + '-' + (1 + timeslotsDate.getMonth()) + '-' + timeslotsDate.getDate();
    this.checkinsService.getTimesolsByPlaygroundAtLocationByDate(8, todaysDateStr).subscribe((locationTimeslots: PlaygroundTimeslots[]) => {
      this.locationTimeslots = [];
      if (locationTimeslots !== null && locationTimeslots !== undefined) {
        this.locationTimeslots = locationTimeslots;
        if (this.locationTimeslots.length > 3) {
          this.locationTimeslots.length = 3;
        }
      }
    });
  }

  oneDayLater() {
    var currentDay = this.dateValue.getDate();
    this.showOneDayBeforeButton = true;
    this.dateValue.setDate(currentDay + 1);
    this.dateString = this.dateValue.toDateString().slice(0,10);
    this.setTimeslotsByDate(this.dateValue);
  }

  oneDayBefore() {
    var currentDay = this.dateValue.getDate();
    this.dateValue.setDate(currentDay - 1);
    if (this.dateValue.getDate() === new Date().getDate()) {
      this.showOneDayBeforeButton = false;
      this.dateString = "Today";
    } else {
      this.dateString = this.dateValue.toDateString().slice(0,10);
    }

    this.setTimeslotsByDate(this.dateValue);
  }
}
