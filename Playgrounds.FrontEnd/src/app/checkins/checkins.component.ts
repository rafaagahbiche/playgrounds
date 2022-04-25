import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { CheckinsService } from '../_services/checkins.service';
import { Timeslot } from '../_models/CheckIn';
import { BsDatepickerConfig } from 'ngx-bootstrap';
@Component({
  selector: 'app-checkins',
  templateUrl: './checkins.component.html',
  styleUrls: ['./checkins.component.scss']
})
export class CheckinsComponent implements OnInit {
  checkinsTimeslotsResult: Timeslot[] = new Array<Timeslot>();
  bsConfig: Partial<BsDatepickerConfig>;
  searchForm: FormGroup;
  dateValue: Date = new Date();
  constructor(private checkinsService: CheckinsService) { }

  ngOnInit() {
    this.getTodaysCheckinTimeslots(this.dateValue);
    this.searchForm = new FormGroup({
      checkinDate: new FormControl(this.dateValue)
    });
    this.onDateInputChange();

    this.bsConfig = {
      containerClass: 'theme-red',
      adaptivePosition: true,
      minDate: new Date(),
      dateInputFormat: 'YYYY-MM-DD'
    };
  }

  onDateInputChange(): void {
    this.searchForm.valueChanges.subscribe(val => {
      if (val !== undefined && val !== null) {
        this.dateValue = val.checkinDate;
        this.getTodaysCheckinTimeslots(val.checkinDate);
      }
    });
  }

  private getTodaysCheckinTimeslots(dateToSearch: Date) {
    let todaysDateStr = dateToSearch.getFullYear() + '-' + (1 + dateToSearch.getMonth()) + '-' + dateToSearch.getDate();
    this.checkinsService.getTimeslotsAtLocationByDate(2, todaysDateStr).subscribe((checkinsTimeSlots: Timeslot[]) => {
      this.checkinsTimeslotsResult = [];
      if (checkinsTimeSlots !== undefined && checkinsTimeSlots !== null) {
        this.checkinsTimeslotsResult = checkinsTimeSlots;
      }
    });
  }

  searchCheckins() {
    if (this.searchForm.value !== undefined && this.searchForm.value !== null) {
      this.getTodaysCheckinTimeslots(this.searchForm.value.checkinDate);
    }
  }
}
