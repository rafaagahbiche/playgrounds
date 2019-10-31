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
  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  memberName = new BehaviorSubject<string>('');
  isLoggedIn = new BehaviorSubject<boolean>(this.jwtHelper.isTokenExpired(this.getMemberToken()));
  currentMemberName = this.memberName.asObservable();
  currentLoggedInStatus = this.isLoggedIn.asObservable();

  constructor(private http: HttpClient) { }

  changeMemberName(memberName: string) {
    this.memberName.next(memberName);
  }

  setLoggedInStatus(status: boolean) {
    this.isLoggedIn.next(status);
  }

  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model).pipe(map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          this.changeMemberName(this.decodedToken.unique_name);
          this.setLoggedInStatus(true);
        }
      }
    ));
  }

  logout() {
    this.setLoggedInStatus(false);
    localStorage.removeItem('token');
    this.decodedToken = null;
    this.changeMemberName('');
  }

  getMemberToken() {
    return localStorage.getItem('token');
  }

  register(member: Member) {
    return this.http.post(this.baseUrl + 'register', member);
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }
}