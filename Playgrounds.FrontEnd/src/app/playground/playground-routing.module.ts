import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PlaygroundComponent } from './playground.component';
import { PhotosGalleryComponent } from './photos/photos-gallery/photos-gallery.component';
import { TimelineComponent } from './timeline/timeline.component';


const routes: Routes = [
  {
    path: '', component: PlaygroundComponent, children: [
      { path: 'photos', component: PhotosGalleryComponent },
      { path: 'timeline', component: TimelineComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PlaygroundRoutingModule { }
