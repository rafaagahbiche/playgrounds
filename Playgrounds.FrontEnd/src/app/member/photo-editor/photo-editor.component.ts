import { Component, OnInit, Input } from '@angular/core';
import { FileUploader, FileItem } from 'ng2-file-upload';
import { Photo } from '../../_models/Photo';
import { PhotosService } from 'src/app/_services/photos.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.scss']
})
export class PhotoEditorComponent implements OnInit {
  uploader: FileUploader;
  @Input() memberPhotos: Photo[];
  hasBaseDropZoneOver = false;
  constructor(private photosService: PhotosService, private authService: AuthService) { }

  ngOnInit() {
    this.initializeUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = this.photosService.createFileUploader(this.authService.getMemberToken());
    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: Photo = JSON.parse(response);
        const photo = {
          id: res.id,
          url: res.url
        };
        this.memberPhotos.push(photo);
      }
    };
  }

  deletePhoto(id: number) {
  }
}
