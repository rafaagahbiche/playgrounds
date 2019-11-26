import { Component, OnInit } from '@angular/core';
import { PlaygroundsService } from '../_services/playgrounds.service';
import { Playground } from '../_models/Playground';
import { AuthService } from '../_services/auth.service';
import { BsDatepickerConfig  } from 'ngx-bootstrap';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { CheckIn } from '../_models/CheckIn';

@Component({
  selector: 'app-playground',
  templateUrl: './playground.component.html',
  styleUrls: ['./playground.component.scss']
})
export class PlaygroundComponent implements OnInit {

  constructor(private playgroundsService: PlaygroundsService, private authService: AuthService, private route: ActivatedRoute) { }
  playground: Playground;
  bsConfig: Partial<BsDatepickerConfig>;
  checkInForm: FormGroup;
  isCheckedIn = false;
  checkInModel: CheckIn;
  checkInsAtPlayground: CheckIn[];
  isLoggedIn = false;

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.playground = data['playground'];
    });

    this.checkInForm = new FormGroup({
      checkInDate: new FormControl('', Validators.required),
      checkInTime: new FormControl(null, Validators.required)
    });

    this.bsConfig = {
      containerClass: 'theme-red',
      adaptivePosition: true,
      minDate: new Date(),
      dateInputFormat: 'YYYY-MM-DD'
    };

    this.getCheckIns();

    this.authService.currentLoggedInStatus.subscribe(isLoggedIn => this.isLoggedIn = isLoggedIn);
  }

  getCheckIns() {
    this.playgroundsService.getCheckInsAtPlayground(this.playground.id).subscribe((checkInsAtPlayground: CheckIn[]) => {
      this.checkInsAtPlayground = checkInsAtPlayground;
    });
  }

  private createDateAndTime(dateFromCalendar: Date, timeFromClock: Date) {
    const dateAndTime = new Date();
      dateAndTime.setDate(dateFromCalendar.getDate());
      dateAndTime.setFullYear(dateFromCalendar.getFullYear());
      dateAndTime.setMonth(dateFromCalendar.getMonth());
      dateAndTime.setHours(timeFromClock.getHours());
      dateAndTime.setMinutes(timeFromClock.getMinutes());
      return dateAndTime;
  }

  checkInMemberToPlayground() {
    if (this.checkInForm.valid) {
      this.checkInModel =  Object.assign({}, this.checkInForm.value);
      this.checkInModel.checkInDate = this.createDateAndTime(new Date(this.checkInForm.value.checkInDate),
                                                 new Date(this.checkInForm.value.checkInTime));
      this.checkInModel.playgroundId = this.playground.id,
      this.playgroundsService.checkInToPlayground(this.checkInModel, this.authService.getMemberToken())
        .subscribe((checkInViewModel: CheckIn) => {
          this.isCheckedIn = true;
      });
    }
  }
}
