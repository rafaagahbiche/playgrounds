import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import {map} from 'rxjs/operators';
import {JwtHelperService} from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/Member';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = environment.apiUrl + 'auth/';
  private jwtHelper = new JwtHelperService();
  private memberName = new BehaviorSubject<string>('');
  currentMemberName = this.memberName.asObservable();
  private loggedInStatus = new BehaviorSubject<boolean>(this.jwtHelper.isTokenExpired(this.getMemberToken()));
  currentLoggedInStatus = this.loggedInStatus.asObservable();
  private memberPhotoUrl = new BehaviorSubject<string>(environment.defaultProfilePicture);
  currentMemberPhotoUrl = this.memberPhotoUrl.asObservable();

  constructor(private http: HttpClient) { }

  setUserPropertiesFromMyDecodedToken() {
    const myToken = this.getMemberToken();
    const myDecodedToken = this.jwtHelper.decodeToken(myToken);
    this.changeMemberName(myDecodedToken.unique_name);
    this.changeMemberPhoto(myDecodedToken.profilePictureUrl);
    this.setLoggedInStatus(true);
  }

  public register(member: Member) {
    return this.http.post(this.baseUrl + 'register', member);
  }

  public login(model: any) {
    return this.http.post(this.baseUrl + 'login', model).pipe(map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          this.setUserPropertiesFromMyDecodedToken();
        }
      }
    ));
  }

  public logout() {
    this.setLoggedInStatus(false);
    this.changeMemberName('');
    this.changeMemberPhoto('');
    localStorage.removeItem('token');
  }

  public getMemberToken() {
    return localStorage.getItem('token');
  }

  public isLoggedIn() {
    const token = this.getMemberToken();
    return !this.jwtHelper.isTokenExpired(token);
  }

  private setLoggedInStatus(status: boolean) {
    this.loggedInStatus.next(status);
  }

  private changeMemberName(memberName: string) {
    this.memberName.next(memberName);
  }

  private changeMemberPhoto(photoUrl: string) {
    if (photoUrl !== null && photoUrl !== undefined && photoUrl !== '') {
      this.memberPhotoUrl.next(photoUrl);
    } else {
      this.memberPhotoUrl.next(environment.defaultProfilePicture);
    }
  }
}
