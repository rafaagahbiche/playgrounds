import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  loggedInUserName: string;
  isLoggedIn = false;
  photoUrl: string;

  constructor(
    private authService: AuthService,
    private router: Router) { }

  ngOnInit(): void {
    this.authService.currentMemberName.subscribe(memberName => this.loggedInUserName =  memberName);
    this.authService.currentLoggedInStatus.subscribe(isLoggedIn => this.isLoggedIn = isLoggedIn);
    this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/home']);
  }
}
