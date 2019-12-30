import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  title = 'Playgrounds Gallery';

  constructor(private authService: AuthService) {

  }

  ngOnInit(): void {
    if (this.authService.isLoggedIn()) {
      this.authService.setUserPropertiesFromMyDecodedToken();
    } else {
      this.authService.logout();
    }
  }
}
