import { Component, OnInit, Input } from '@angular/core';
import { CheckInToDisplay } from 'src/app/_models/CheckIn';

@Component({
  selector: 'app-players-checkins',
  templateUrl: './players-checkins.component.html',
  styleUrls: ['./players-checkins.component.scss']
})
export class PlayersCheckinsComponent implements OnInit {
  @Input() playgroundCheckIns: CheckInToDisplay[];
  @Input() fourMostRecentCheckIns: CheckInToDisplay[];
  @Input() numberOfCheckIns: number;
  @Input() moreThanFourCheckIns: boolean;
  showCheckinsDetails = false;

  constructor() { }

  ngOnInit() {
  }
}
