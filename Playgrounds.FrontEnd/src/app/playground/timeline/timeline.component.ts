import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TimelinePost } from 'src/app/_models/TimelinePost';
import { AuthService } from 'src/app/_services/auth.service';
import { PhotosService } from 'src/app/_services/photos.service';
import { NgxSpinnerService } from 'ngx-spinner';

  @Component({
    selector: 'app-timeline',
    templateUrl: './timeline.component.html',
    styleUrls: ['./timeline.component.scss'],
  })

  export class TimelineComponent implements OnInit {
    timelinePosts: TimelinePost[] = new Array<TimelinePost>();
    userName: string;
    userPhotoUrl: string;
    playgroundId: number;
    isLoggedIn: boolean;
    constructor(
      private authService: AuthService,
      private photoServce: PhotosService,
      private route: ActivatedRoute,
      private spinner: NgxSpinnerService) { }

    ngOnInit() {
      this.playgroundId = this.route.snapshot.parent.params.id;
      this.timelinePosts = new Array<TimelinePost>();
      this.spinner.show('timeline-spinner');
      setTimeout(() => {
        this.photoServce.getPlaygroundPosts(this.playgroundId).subscribe((posts: TimelinePost[]) => {
          this.spinner.hide('timeline-spinner');
          if (posts === null || posts === undefined) {
            return;
          }

          this.timelinePosts = posts;
        });
      }, 3000);
      this.authService.currentMemberName.subscribe(name => this.userName = name);
      this.authService.currentMemberPhotoUrl.subscribe(photoUrl => this.userPhotoUrl = photoUrl);
      this.authService.currentLoggedInStatus.subscribe(isLoggedIn => this.isLoggedIn = isLoggedIn);
    }
  }
