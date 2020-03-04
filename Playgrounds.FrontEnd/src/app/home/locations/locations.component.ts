import { Component, OnInit } from '@angular/core';
import { PlaygroundsService } from 'src/app/_services/playgrounds.service';
import { Playground } from 'src/app/_models/Playground';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-locations',
  templateUrl: './locations.component.html',
  styleUrls: ['./locations.component.scss']
})
export class LocationsComponent implements OnInit {

  constructor(private playgroundsService: PlaygroundsService, private route: ActivatedRoute) { }
  playgrounds: Playground[];

  ngOnInit() {
    this.playgroundsService.getPlaygroundsByLocationId(2).subscribe((playgrounds: Playground[]) => {
      if (playgrounds) {
        this.playgrounds = playgrounds;
      }
    });
  }
}
