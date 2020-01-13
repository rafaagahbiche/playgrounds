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
  minDate = new Date('0001-01-01T00:00:00');

  constructor() { }

  ngOnInit() {
  }

  comapreDates(date1: Date) {
    return new Date(date1) > this.minDate;
  }

}
