import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { CheckIn } from '../_models/CheckIn';

@Injectable({
  providedIn: 'root'
})
export class PlaygroundsService {

  constructor(private http: HttpClient) { }
  private playgroundApiUrl = environment.apiUrl + 'playgrounds/';
  private checkinApiUrl = environment.apiUrl + 'checkins/';
  private memberCheckinApiUrl = environment.apiUrl + 'member-checkins/';
  private locationApiUrl = environment.apiUrl + 'locations/';

  getAllLoacations() {
    return this.http.get(this.locationApiUrl);
  }

  getPlaygroundsByLocationId(locationId: number) {
    return this.http.get(this.playgroundApiUrl + 'locations/' + locationId);
  }

  getPlaygroundById(playgroundId: number) {
    return this.http.get(this.playgroundApiUrl + playgroundId);
  }

  getCheckInsAtPlayground(playgroundId: number) {
    return this.http.get(this.checkinApiUrl + 'playgrounds/' + playgroundId);
  }

  checkInToPlayground(checkinModel: CheckIn, token: any) {
    if (token !== null) {
      const httpOptions = {
        headers: new HttpHeaders({
          'Authorization': 'Bearer ' + token
        })
      };

      return this.http.post(this.memberCheckinApiUrl, checkinModel, httpOptions);
    }
  }
}
