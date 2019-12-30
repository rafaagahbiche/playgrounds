import { Component, OnInit, Input } from '@angular/core';
import { TimelinePost } from 'src/app/_models/TimelinePost';

@Component({
  selector: 'app-timeline-forms',
  templateUrl: './timeline-forms.component.html',
  styleUrls: ['./timeline-forms.component.scss']
})
export class TimelineFormsComponent implements OnInit {
  @Input() mainPosts: TimelinePost[];
  @Input() userName: string;
  @Input() userPhotoUrl: string;
  @Input() playgroundId: number;

  constructor() { }

  ngOnInit() {
  }

}
