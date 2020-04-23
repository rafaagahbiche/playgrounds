import { Component, OnInit, Input } from '@angular/core';
import { Timeslot } from 'src/app/_models/CheckIn';

@Component({
  selector: 'app-playground-timeslots',
  templateUrl: './playground-timeslots.component.html',
  styleUrls: ['./playground-timeslots.component.scss']
})
export class HomePlaygroundTimeslotsComponent implements OnInit {

  @Input() playgroundTimeslots : Timeslot[] = new Array();
  @Input() playgroundId: number;
  @Input() playgroundAddress: string;
  @Input() playgroundPhotoUrl: string;
  pixelsToTranslate: number = 0;
  rightDisabled: boolean = true;
  private sliderWidth : number = 220;

  constructor() { }

  ngOnInit() {
    if (this.playgroundTimeslots.length > 3) {
      this.playgroundTimeslots.length = 3;
    } 
  }

  goLeft() {
    if (this.pixelsToTranslate < 0) {
      this.pixelsToTranslate = this.pixelsToTranslate + this.sliderWidth;
    }
  }
  goRight() {
    if (this.pixelsToTranslate > - this.sliderWidth*(this.playgroundTimeslots.length -1)) {
      this.pixelsToTranslate = this.pixelsToTranslate - this.sliderWidth;
    }
  }
}
