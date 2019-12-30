import { Component, OnInit, Input } from '@angular/core';
import { PhotosService } from 'src/app/_services/photos.service';
import { Photo } from 'src/app/_models/Photo';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-photos-gallery',
  templateUrl: './photos-gallery.component.html',
  styleUrls: ['./photos-gallery.component.scss']
})
export class PhotosGalleryComponent implements OnInit {
  playgroundId: number;
  playgroundPhotos: Photo[];
  errorWhenRetreivingPhotos: string;
  constructor(private photosService: PhotosService, private route: ActivatedRoute) { }

  ngOnInit() {
    // console.log(this.route.snapshot);
    this.playgroundId = this.route.snapshot.parent.params.id;
    this.getPhotos();
  }

  getPhotos() {
    this.photosService.getPlaygroundPhotos(this.playgroundId).subscribe((response: Photo[]) => {
      this.playgroundPhotos = response;
    },
    error => {
      this.errorWhenRetreivingPhotos = error;
    });
  }
}
