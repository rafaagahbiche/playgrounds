import { Component, OnInit } from '@angular/core';
import { PhotosService } from 'src/app/_services/photos.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-photos-manager',
  templateUrl: './photos-manager.component.html',
  styleUrls: ['./photos-manager.component.scss']
})
export class PhotosManagerComponent implements OnInit {
  memberPhotos: any;
  errorWhenRetreivingPhotos: string;
  constructor(
    private photosService: PhotosService,
    private authService: AuthService) { }

  ngOnInit() {
    this.getPhoto();
  }

  getPhoto() {
    this.photosService.getMemberPhotos(this.authService.getMemberToken()).subscribe(response => {
      this.memberPhotos = response;
    },
    error => {
      this.errorWhenRetreivingPhotos = error;
    });
  }
}
