import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { CheckIn } from '../_models/CheckIn';
import Utils from './utils';

@Injectable({
  providedIn: 'root'
})
export class CheckinsService {

  constructor(private http: HttpClient) { }

  private checkinApiUrl = environment.apiUrl + 'checkins/';
  private memberCheckinApiUrl = environment.apiUrl + 'member-checkins/';

  public getCheckInsAtPlayground(playgroundId: number) {
    return this.http.get(this.checkinApiUrl + 'playgrounds/' + playgroundId);
  }

  public getCheckInsAtPlaygroundByDate(playgroundId: number, dateTime: Date) {
    return this.http.get(this.checkinApiUrl + 'playgrounds/' + playgroundId + '/' + dateTime);
  }

  public getCheckinsSlotsAtPlaygroundByDate(playgroundId: number, dateTime: string) {
    return this.http.get(this.checkinApiUrl + 'playgrounds/' + playgroundId + '/slots/' + dateTime);
  }

  public getCheckinsAtPlaygroundBetweenTwoDate(playgroundId: number, startDateTime: Date, endDateTime: Date) {
    return this.http.get(this.checkinApiUrl + 'playgrounds/' + playgroundId + '/' + startDateTime + '/' + endDateTime);
  }

  public checkInToPlayground(checkinModel: CheckIn, token: any) {
    if (token !== null) {
      let httpOptions = Utils.getHttpOptionsHeader(token);
      return this.http.post(this.memberCheckinApiUrl, checkinModel, httpOptions);
    }
  }

  public cancelCheckin(checkinId: number, token: any) {
    if (token !== null) {
      let httpOptions = Utils.getHttpOptionsHeader(token);
      return this.http.delete(this.memberCheckinApiUrl + checkinId, httpOptions);
    }
  }
}
