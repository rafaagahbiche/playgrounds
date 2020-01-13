import { Component, OnInit } from '@angular/core';
import { CheckInToDisplay } from 'src/app/_models/CheckIn';
import { ActivatedRoute } from '@angular/router';
import { PlaygroundsService } from 'src/app/_services/playgrounds.service';

@Component({
  selector: 'app-players-checkins',
  templateUrl: './players-checkins.component.html',
  styleUrls: ['./players-checkins.component.scss']
})
export class PlayersCheckinsComponent implements OnInit {
  playgroundId: number;
  playgroundCheckIns: CheckInToDisplay[];
  numberOfCheckIns: number;
  showCheckinsDetails = false;

  constructor(private playgroundsService: PlaygroundsService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.playgroundId = this.route.snapshot.parent.params.id;
    this.getCheckIns();
  }

  private getCheckIns() {
    this.playgroundsService.getCheckInsAtPlayground(this.playgroundId).subscribe((checkInsAtPlayground: CheckInToDisplay[]) => {
      if (checkInsAtPlayground !== undefined) {
        this.playgroundCheckIns = checkInsAtPlayground;
        this.numberOfCheckIns = this.playgroundCheckIns.length;
      }
    });
  }
}
