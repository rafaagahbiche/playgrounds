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
  errorWhileDeletingPhoto: string;
  errorWhileUpdatingPhoto: string;
  editMode = false;

  constructor(private photosService: PhotosService, private authService: AuthService) { }

  ngOnInit() {
  }

  deletePhoto(photoId: number) {
    this.photosService.deletePhoto(photoId, this.authService.getMemberToken()).subscribe(() => {
      const photoToDeleteIndex = this.memberPhotos.findIndex(p => p.id === photoId);
      this.memberPhotos.splice(photoToDeleteIndex, 1);
    },
    error => {
      this.errorWhileDeletingPhoto = error;
    });
  }
}
