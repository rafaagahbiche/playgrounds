import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { CheckIn, CheckInToDisplay } from 'src/app/_models/CheckIn';
import { AuthService } from 'src/app/_services/auth.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { CheckinsService } from 'src/app/_services/checkins.service';

@Component({
  selector: 'app-checkin-form',
  templateUrl: './checkin-form.component.html',
  styleUrls: ['./checkin-form.component.scss']
})
export class CheckinFormComponent implements OnInit {
  @Input() userName: string;
  @Input() userPhotoUrl: string;
  @Input() playgroundId: number;
  @Output() disposed = new EventEmitter<boolean>();

  bsConfig: Partial<BsDatepickerConfig>;
  checkinForm: FormGroup;
  isCheckedIn = false;
  checkInModel: CheckIn;

  constructor(
    private authService: AuthService,
    private checkinsService: CheckinsService,
    private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.checkinForm = new FormGroup({
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

  checkinMemberToPlayground() {
    if (this.checkinForm.valid) {
      this.spinner.show('checkin-post-spinner');
      this.checkInModel =  Object.assign({}, this.checkinForm.value);
      this.checkInModel.checkInDate = this.createDateAndTime(new Date(this.checkinForm.value.checkInDate),
                                                 new Date(this.checkinForm.value.checkInTime));
      this.checkInModel.playgroundId = this.playgroundId,
      this.checkinsService.checkInToPlayground(this.checkInModel, this.authService.getMemberToken())
        .subscribe((checkInViewModel: CheckInToDisplay) => {
          if (checkInViewModel !== null && checkInViewModel !== undefined) {
            this.isCheckedIn = true;
            const timelinePost = {
              authorLoginName: this.userName,
              authorProfilePictureUrl: this.userPhotoUrl,
              checkInDate: checkInViewModel.checkInDate
            };
          }
          this.spinner.hide('checkin-post-spinner');
          this.disposed.emit(true);
      });
    }
  }

  closeCheckinForm() {
    this.disposed.emit(true);
  }
}
