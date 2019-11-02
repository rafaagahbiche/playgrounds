import { Component, OnInit, Input } from '@angular/core';
import { Photo } from 'src/app/_models/Photo';

@Component({
  selector: 'app-single-gallery-photo',
  templateUrl: './single-gallery-photo.component.html',
  styleUrls: ['./single-gallery-photo.component.scss']
})
export class SingleGalleryPhotoComponent implements OnInit {
  @Input() photo: any;
  photoModel: Photo;
  photoUrl: string;
  constructor() { }

  ngOnInit() {
    this.photoUrl = this.photo.url;
    this.photoModel = {
      id: this.photo.id,
      url: this.photo.url,
      publicId: this.photo.publicId
    };
  }

}
