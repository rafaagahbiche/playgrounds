import { Component, OnInit, Input } from '@angular/core';
import { TimelinePost } from 'src/app/_models/TimelinePost';
import { fadeInAnimation } from 'src/app/_animations/fadeInAnimation';


@Component({
  selector: 'app-timeline-posts',
  templateUrl: './timeline-posts.component.html',
  styleUrls: ['./timeline-posts.component.scss'],
  animations: [fadeInAnimation]
})
export class TimelinePostsComponent implements OnInit {
  @Input() mainPosts: TimelinePost[];
  @Input() playgroundId: number;
  minDate = new Date(-8640000000000000);

  constructor() { }

  ngOnInit() {
  }

}
