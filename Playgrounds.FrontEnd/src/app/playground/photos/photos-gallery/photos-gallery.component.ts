import { Component, OnInit, Input } from '@angular/core';
import { PhotosService } from 'src/app/_services/photos.service';
import { Photo } from 'src/app/_models/Photo';
import { ActivatedRoute } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { fadeInAnimation } from 'src/app/_animations/fadeInAnimation';

@Component({
  selector: 'app-photos-gallery',
  templateUrl: './photos-gallery.component.html',
  styleUrls: ['./photos-gallery.component.scss'],
  animations:  [fadeInAnimation]
})
export class PhotosGalleryComponent implements OnInit {
  playgroundId: number;
  playgroundPhotos: Photo[];
  errorWhenRetreivingPhotos: string;
  constructor(
    private photosService: PhotosService,
    private route: ActivatedRoute,
    private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.playgroundId = this.route.snapshot.parent.params.id;
    this.getPhotos();
  }

  getPhotos() {
    this.spinner.show('photos-gallery-spinner');
    setTimeout(() => {
      this.photosService.getPlaygroundPhotos(this.playgroundId).subscribe((response: Photo[]) => {
        this.spinner.hide('photos-gallery-spinner');
        this.playgroundPhotos = response;
      },
      error => {
        this.spinner.hide('photos-gallery-spinner');
        this.errorWhenRetreivingPhotos = error;
      });
    }, 2000);
  }
}
