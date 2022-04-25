import { Component, OnInit } from '@angular/core';
import { Playground } from '../_models/Playground';
import { PlaygroundsService } from '../_services/playgrounds.service';

@Component({
  selector: 'app-playgrounds',
  templateUrl: './playgrounds.component.html',
  styleUrls: ['./playgrounds.component.scss']
})
export class PlaygroundsComponent implements OnInit {
  playgrounds: Playground[] = new Array<Playground>();
  constructor(private playgroundsService: PlaygroundsService) { }

  ngOnInit() {
    this.playgroundsService.getPlaygroundsByLocationId(8).subscribe((playgrounds: Playground[]) => {
      if (playgrounds !== null && playgrounds !== undefined) {
        this.playgrounds = playgrounds.filter(p => p.mainPhotoUrl !== null);
      }
    });
  }

}
