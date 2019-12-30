import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { CheckIn } from 'src/app/_models/CheckIn';
import { PlaygroundsService } from 'src/app/_services/playgrounds.service';
import { AuthService } from 'src/app/_services/auth.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-checkin-form',
  templateUrl: './checkin-form.component.html',
  styleUrls: ['./checkin-form.component.scss']
})
export class CheckinFormComponent implements OnInit {
  @Input() userName: string;
  @Input() userPhotoUrl: string;
  @Input() playgroundId: number;

  protected bsConfig: Partial<BsDatepickerConfig>;
  protected checkInForm: FormGroup;
  protected isCheckedIn = false;
  protected checkInModel: CheckIn;

  constructor(
    private authService: AuthService,
    private playgroundsService: PlaygroundsService,
    private spinner: NgxSpinnerService) { }

  ngOnInit() {
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
      this.spinner.show();
      this.checkInModel =  Object.assign({}, this.checkInForm.value);
      this.checkInModel.checkInDate = this.createDateAndTime(new Date(this.checkInForm.value.checkInDate),
                                                 new Date(this.checkInForm.value.checkInTime));
      this.checkInModel.playgroundId = this.playgroundId,
      this.playgroundsService.checkInToPlayground(this.checkInModel, this.authService.getMemberToken())
        .subscribe((checkInViewModel: CheckIn) => {
          if (checkInViewModel !== null && checkInViewModel !== undefined) {
            this.isCheckedIn = true;
            this.spinner.hide();
          }
      });
    }
  }
}
