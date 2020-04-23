import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PlaygroundComponent } from './playground.component';
import { PhotosGalleryComponent } from './photos/photos-gallery/photos-gallery.component';
import { TimelineComponent } from './timeline/timeline.component';
import { PlaygroundCheckinsComponent } from './checkins/checkins.component';


const routes: Routes = [
  { path: '', component: PlaygroundComponent, children: [
    { path: '', component: TimelineComponent },
    { path: 'timeline', component: TimelineComponent },
    { path: 'checkins', component: PlaygroundCheckinsComponent },
    { path: 'photos', component: PhotosGalleryComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PlaygroundRoutingModule { }
