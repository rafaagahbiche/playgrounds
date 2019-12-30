import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BsModalRef, TabHeadingDirective } from 'ngx-bootstrap';
import { FileUploader } from 'ng2-file-upload';
import { PhotosService } from 'src/app/_services/photos.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-add-photo-form',
  templateUrl: './add-photo-form.component.html',
  styleUrls: ['./add-photo-form.component.scss']
})
export class AddPhotoFormComponent implements OnInit {
  addPhotoForm: FormGroup;
  photoUploader: FileUploader;
  title: string;
  constructor(
    private photosService: PhotosService,
    private authService: AuthService,
    private bsModalRef: BsModalRef) { }

  ngOnInit() {
    console.log(this.title);
    this.addPhotoForm = new FormGroup({});
  }

  openPhotoUploaderModal() {
    const initialState = {
      title: 'Modal with component'
    };

    // this.bsModalRef = this.modalService.show(AddPhotoFormComponent, {initialState});
  }

  AddPhotoToPlayground() {
    this.bsModalRef.hide();
  }

  close() {
    this.bsModalRef.hide();
  }
}
