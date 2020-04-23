import { Component, OnInit } from '@angular/core';
import { CheckinsService } from 'src/app/_services/checkins.service';
import { Timeslot, PlaygroundTimeslots } from 'src/app/_models/CheckIn';

@Component({
  selector: 'app-location-checkins',
  templateUrl: './location-checkins.component.html',
  styleUrls: ['./location-checkins.component.scss']
})
export class LocationCheckinsComponent implements OnInit {

  constructor(private checkinsService: CheckinsService) { }

  locationTimeslots : PlaygroundTimeslots[] = new Array();

  ngOnInit() {
    var todaysDate = new Date();
    var todaysDateStr = todaysDate.getFullYear() + '-' + (1 + todaysDate.getMonth()) + '-' + todaysDate.getDate();

    this.checkinsService.getCheckinsSlotsAtLocationByDate(2, todaysDateStr).subscribe((locationTimeslots: PlaygroundTimeslots[]) => {
      if (locationTimeslots !== null && locationTimeslots !== undefined) {
        this.locationTimeslots = locationTimeslots;
        if (this.locationTimeslots.length > 3) {
          this.locationTimeslots.length = 3;
        }
      }
    });
  }

  
}
