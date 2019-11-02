import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  title = 'Playgrounds Gallery';
  jwtHelper = new JwtHelperService();

  constructor(private authService: AuthService) {

  }

  ngOnInit(): void {
    const token = localStorage.getItem('token');
    if (this.authService.loggedIn()) {
      this.authService.setLoggedInStatus(true);
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
      this.authService.changeMemberName(this.authService.decodedToken.unique_name);
    } else {
      this.authService.logout();
    }
  }
}
