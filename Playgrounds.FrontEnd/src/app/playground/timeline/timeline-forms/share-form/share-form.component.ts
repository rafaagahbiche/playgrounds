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

  protected descriptionPlaceholderText = 'Share your experience in this playground...';
  protected postDescriptionText = '';
  protected photoUploader: FileUploader;
  protected shareSectionFocused = false;
  protected canBePosted = false;
  protected errorText: string;

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
      this.spinner.hide();
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
        this.spinner.hide();
      }
    };
  }

  protected clearPhotoToPost() {
    this.photoUploader.clearQueue();
    this.canBePosted = this.postDescriptionText !== '';
  }

  protected onPosteDescriptionValueChanged() {
    if (this.postDescriptionText.length > 0) {
      this.canBePosted = true;
    } else {
      const myPhoto = this.photoUploader.getNotUploadedItems();
      this.canBePosted = myPhoto !== null && myPhoto !== undefined && myPhoto.length > 0;
    }
  }

  protected sharePostToPlayground() {
    const photoDescription = this.postDescriptionText !== '' ? this.postDescriptionText : '';
    this.photoUploader.setOptions({
      additionalParameter: {
        playgroundId: this.playgroundId,
        description: photoDescription
      }
    });
    this.spinner.show();
    setTimeout(() => {
      this.photoUploader.uploadAll();
    }, 3000);
  }
}
