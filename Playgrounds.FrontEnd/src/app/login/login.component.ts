import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  model: any = {};
  constructor(
    private authService: AuthService,
    private router: Router,
    private spinner: NgxSpinnerService) {
  }

  ngOnInit() {
  }

  login() {
    this.spinner.show();
    this.authService.login(this.model).subscribe(next => {
      this.spinner.hide();
    },
     error => {
      this.spinner.hide();
      console.log(error);
    }, () => {
      this.router.navigate(['/member/photos-manager']);
    });
  }
}
