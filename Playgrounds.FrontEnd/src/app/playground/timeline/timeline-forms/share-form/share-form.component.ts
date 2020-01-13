import { Component, OnInit, Input } from '@angular/core';
import { PhotosService } from 'src/app/_services/photos.service';
import { AuthService } from 'src/app/_services/auth.service';
import { FileUploader } from 'ng2-file-upload';
import { TimelinePost } from 'src/app/_models/TimelinePost';
import { NgxSpinnerService } from 'ngx-spinner';
import { Photo } from 'src/app/_models/Photo';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-share-form',
  templateUrl: './share-form.component.html',
  styleUrls: ['./share-form.component.scss']
})

export class ShareFormComponent implements OnInit {
  @Input() mainPosts: TimelinePost[];
  @Input() userName: string;
  @Input() userPhotoUrl: string;
  @Input() playgroundId: number;

  descriptionPlaceholderText = 'Share your experience in this playground...';
  postDescriptionText = '';
  photoUploader: FileUploader;
  shareSectionFocused = false;
  canBePosted = false;
  errorText: string;

  constructor(
    private authService: AuthService,
    private photosService: PhotosService,
    private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.setPhotoUploader();
  }

  private setPhotoUploader() {
    this.photoUploader = this.photosService.createFileUploader(this.authService.getMemberToken());
    this.photoUploader.onAfterAddingFile = (file) => {
      this.canBePosted = true;
      file.withCredentials = false;
    };
    this.photoUploader.onErrorItem = (item, response, status, headers) => {
      this.postDescriptionText = '';
      this.spinner.hide('share-form-spinner');
      this.errorText = response;
    };

    this.photoUploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: Photo = JSON.parse(response);
        const timelinePost = {
          authorLoginName: this.userName,
          authorProfilePictureUrl: this.userPhotoUrl,
          url: res.url,
          description: res.description
        };
        this.postDescriptionText = '';
        this.mainPosts.unshift(timelinePost);
        this.spinner.hide('share-form-spinner');
      }
    };
  }

  clearPhotoToPost() {
    this.photoUploader.clearQueue();
    this.canBePosted = false;
    // this.canBePosted = this.postDescriptionText !== '';
  }

  onPosteDescriptionValueChanged() {
    // Uncomment when comments with no photos can be added to a Playground timeline.
    // if (this.postDescriptionText.length > 0) {
    //   this.canBePosted = true;
    // } else {
    //   const myPhoto = this.photoUploader.getNotUploadedItems();
    //   this.canBePosted = myPhoto !== null && myPhoto !== undefined && myPhoto.length > 0;
    // }
  }

  sharePostToPlayground() {
    const photoDescription = this.postDescriptionText !== '' ? this.postDescriptionText : '';
    this.photoUploader.setOptions({
      additionalParameter: {
        playgroundId: this.playgroundId,
        description: photoDescription
      }
    });
    this.spinner.show('share-form-spinner');
    setTimeout(() => {
      this.photoUploader.uploadAll();
    }, 3000);
  }
}
