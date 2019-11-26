import { Component, OnInit, Input } from '@angular/core';
import { Photo } from 'src/app/_models/Photo';

@Component({
  selector: 'app-single-gallery-photo',
  templateUrl: './single-gallery-photo.component.html',
  styleUrls: ['./single-gallery-photo.component.scss']
})
export class SingleGalleryPhotoComponent implements OnInit {
  @Input() photo: Photo;
  constructor() { }

  ngOnInit() {
  }

}
