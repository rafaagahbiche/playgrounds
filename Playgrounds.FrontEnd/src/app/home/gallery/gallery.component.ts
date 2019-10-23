import { Component, OnInit } from '@angular/core';
import { PhotosService } from 'src/app/_services/photos.service';

@Component({
  selector: 'app-gallery',
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.scss']
})
export class GalleryComponent implements OnInit {
  galleryPhotos: any;

  constructor(private photosService: PhotosService) { }

  ngOnInit() {
    this.getPhotos();
  }

  getPhotos() {
    this.photosService.getRecentPhotos(5).subscribe(response => {
      if (response) {
        this.galleryPhotos = response;
      }
    });
  }

}
