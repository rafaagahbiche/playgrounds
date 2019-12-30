import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TimelinePost } from 'src/app/_models/TimelinePost';
import { AuthService } from 'src/app/_services/auth.service';
import { PhotosService } from 'src/app/_services/photos.service';

  @Component({
    selector: 'app-timeline',
    templateUrl: './timeline.component.html'
  })

  export class TimelineComponent implements OnInit {
    protected timelinePosts: TimelinePost[];
    protected userName: string;
    protected userPhotoUrl: string;
    protected playgroundId: number;
    protected isLoggedIn: boolean;
    constructor(
      private authService: AuthService,
      private photoServce: PhotosService,
      private route: ActivatedRoute) { }

    ngOnInit() {
      this.playgroundId = this.route.snapshot.parent.params.id;
      this.timelinePosts = new Array<TimelinePost>();
      this.photoServce.getPlaygroundPosts(this.playgroundId).subscribe((posts: TimelinePost[]) => {
        this.timelinePosts = posts;
      });

      this.authService.currentMemberName.subscribe(name => this.userName = name);
      this.authService.currentMemberPhotoUrl.subscribe(photoUrl => this.userPhotoUrl = photoUrl);
      this.authService.currentLoggedInStatus.subscribe(isLoggedIn => this.isLoggedIn = isLoggedIn);
    }
  }
