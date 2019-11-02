import { Component, OnInit, Input } from '@angular/core';
import { Photo } from 'src/app/_models/Photo';
import { PhotosService } from 'src/app/_services/photos.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-photos',
  templateUrl: './photos.component.html',
  styleUrls: ['./photos.component.scss']
})
export class PhotosComponent implements OnInit {
  @Input() memberPhotos: Photo[];

  constructor(private photosService: PhotosService, private authService: AuthService) { }

  ngOnInit() {
  }

  deletePhoto(publicId: string) {
    this.photosService.deltePhoto(publicId, this.authService.getMemberToken()).subscribe(() => {
      const photoToDeleteIndex = this.memberPhotos.findIndex(p => p.publicId === publicId);
      this.memberPhotos.splice(photoToDeleteIndex, 1);
    },
    error => {
      console.log('error when deleting');
    });
  }
}
