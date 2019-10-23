import { Component, OnInit, Input } from '@angular/core';
import { Photo } from 'src/app/_models/Photo';

@Component({
  selector: 'app-photos',
  templateUrl: './photos.component.html',
  styleUrls: ['./photos.component.scss']
})
export class PhotosComponent implements OnInit {
  @Input() memberPhotos: Photo[];

  constructor() { }

  ngOnInit() {
  }
}
