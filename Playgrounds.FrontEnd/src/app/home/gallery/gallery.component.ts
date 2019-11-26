import { Component, OnInit } from '@angular/core';
import { PhotosService } from 'src/app/_services/photos.service';
import { Photo } from 'src/app/_models/Photo';

@Component({
  selector: 'app-gallery',
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.scss']
})
export class GalleryComponent implements OnInit {
  galleryPhotos: Photo[];

  constructor(private photosService: PhotosService) { }

  ngOnInit() {
    this.getPhotos();
  }

  getPhotos() {
    this.photosService.getRecentPhotos(5).subscribe((photos: Photo[]) => {
      if (photos) {
        this.galleryPhotos = photos;
      }
    });
  }

}
