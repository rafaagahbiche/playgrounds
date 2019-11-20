import { Component, OnInit, Input } from '@angular/core';
import { Photo } from 'src/app/_models/Photo';
import { PhotosService } from 'src/app/_services/photos.service';
import { AuthService } from 'src/app/_services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl } from '@angular/forms';
import { strictEqual } from 'assert';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.scss']
})
export class PhotoEditorComponent implements OnInit {
  memberPhoto: Photo;
  errorWhileUpdatingPhoto: string;
  updatePhotoForm: FormGroup;

  constructor(private photoService: PhotosService, private authService: AuthService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.memberPhoto = data['photo'];
      console.log(this.memberPhoto);
    });

    this.updatePhotoForm = new FormGroup({
      description: new FormControl(this.memberPhoto.description)
    });
  }

  updatePhoto() {
    const photoModel = Object.assign({}, this.updatePhotoForm.value);
    this.memberPhoto.description = photoModel.description;
    console.log(this.memberPhoto);
    this.photoService.updatePhoto(this.memberPhoto, this.authService.getMemberToken()).subscribe((photoUpdated: Photo) => {
      this.memberPhoto = photoUpdated;
    },
    error => {
      this.errorWhileUpdatingPhoto = error;
    });
  }
}
