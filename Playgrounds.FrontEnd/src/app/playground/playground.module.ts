import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { PlaygroundComponent } from './playground.component';
import { PhotosGalleryComponent } from './photos/photos-gallery/photos-gallery.component';
import { AddPhotoFormComponent } from './photos/add-photo-form/add-photo-form.component';
import { TimelineComponent } from './timeline/timeline.component';
import { CheckinFormComponent } from './timeline/timeline-forms/checkin-form/checkin-form.component';
import { TimelineFormsComponent } from './timeline/timeline-forms/timeline-forms.component';
import { TimelinePostsComponent } from './timeline/timeline-posts/timeline-posts.component';
import { ShareFormComponent } from './timeline/timeline-forms/share-form/share-form.component';
import { PlayersCheckinsComponent } from './timeline/players-checkins/players-checkins.component';
import { PlaygroundRoutingModule } from './playground-routing.module';

import { BsDatepickerModule, TimepickerModule, TabsModule, ModalModule, BsModalRef  } from 'ngx-bootstrap';
// import { BrowserModule } from '@angular/platform-browser';
// import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FileUploadModule } from 'ng2-file-upload';
import { NgxSpinnerModule } from 'ngx-spinner';

@NgModule({
  declarations: [
    PlaygroundComponent,
    PhotosGalleryComponent,
    AddPhotoFormComponent,
    TimelineComponent,
    TimelineFormsComponent,
    TimelinePostsComponent,
    ShareFormComponent,
    CheckinFormComponent,
    PlayersCheckinsComponent
  ],
  imports: [
    CommonModule,
    BsDatepickerModule,
    // BrowserModule,
    // BrowserAnimationsModule,
    TimepickerModule,
    TabsModule,
    FormsModule,
    ReactiveFormsModule,
    PlaygroundRoutingModule,
    FileUploadModule,
    NgxSpinnerModule,
    ModalModule.forRoot()
  ],
  entryComponents: [
    AddPhotoFormComponent
  ],
  providers: [
    BsModalRef
  ]
})
export class PlaygroundModule { }
