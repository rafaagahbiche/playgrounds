import { Component, OnInit, Input } from '@angular/core';
import { TimeslotSelectionService } from 'src/app/_services/timeslot-selection.service';

@Component({
  selector: 'app-timeslot-tab',
  templateUrl: './timeslot-tab.component.html',
  styleUrls: ['./timeslot-tab.component.scss']
})
export class TimeslotTabComponent implements OnInit {
  @Input() startsAt: Date;
  @Input() tabIndex: number;
  currentSelectedIndex: number = 1;
  showTimeslotDetails: boolean;

  constructor(private timeslotSelectionService: TimeslotSelectionService) { }

  ngOnInit() {
    this.timeslotSelectionService.currentTimeslotSelected.subscribe(timeslotSelected => {
      this.showTimeslotDetails = timeslotSelected !== undefined 
                              && timeslotSelected !== null
                              && (new Date(this.startsAt).getTime()).toString() === timeslotSelected;
    });
  }

  setShowDetails() {
    this.timeslotSelectionService.setTimeslotSelection((new Date(this.startsAt)).getTime().toString());
    this.timeslotSelectionService.setPixelsToTranslate(400*(1 - this.tabIndex));
  }
}
