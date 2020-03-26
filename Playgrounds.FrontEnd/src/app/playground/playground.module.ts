import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { PlaygroundComponent } from './playground.component';
import { PhotosGalleryComponent } from './photos/photos-gallery/photos-gallery.component';
import { AddPhotoFormComponent } from './photos/add-photo-form/add-photo-form.component';
import { TimelineComponent } from './timeline/timeline.component';
import { CheckinFormComponent } from './checkins/checkin-form/checkin-form.component';
import { TimelineFormsComponent } from './timeline/timeline-forms/timeline-forms.component';
import { TimelinePostsComponent } from './timeline/timeline-posts/timeline-posts.component';
import { ShareFormComponent } from './timeline/timeline-forms/share-form/share-form.component';
import { CheckinsComponent } from './checkins/checkins.component';
import { PlayersCheckinsComponent } from './checkins/players-checkins/players-checkins.component';
import { UpcomingCheckinsComponent } from './checkins/upcoming-checkins/upcoming-checkins.component';
import { SingleTimeSlotComponent } from './checkins/upcoming-checkins/single-time-slot/single-time-slot.component';
import { PlaygroundRoutingModule } from './playground-routing.module';

import { BsDatepickerModule, TimepickerModule, TabsModule, ModalModule, BsModalRef  } from 'ngx-bootstrap';
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
    CheckinsComponent,
    CheckinFormComponent,
    UpcomingCheckinsComponent,
    SingleTimeSlotComponent,
    PlayersCheckinsComponent
  ],
  imports: [
    CommonModule,
    TimepickerModule,
    TabsModule,
    FormsModule,
    ReactiveFormsModule,
    PlaygroundRoutingModule,
    FileUploadModule,
    NgxSpinnerModule,
    BsDatepickerModule.forRoot(),
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
