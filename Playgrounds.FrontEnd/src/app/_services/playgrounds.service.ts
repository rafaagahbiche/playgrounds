import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PlaygroundsService {

  constructor(private http: HttpClient) { }
  private playgroundApiUrl = environment.apiUrl + 'playgrounds/';
  private locationApiUrl = environment.apiUrl + 'locations/';

  public getAllLoacations() {
    return this.http.get(this.locationApiUrl);
  }

  public getPlaygroundsByLocationId(locationId: number) {
    return this.http.get(this.playgroundApiUrl + 'locations/' + locationId);
  }

  public getPlaygroundById(playgroundId: number) {
    return this.http.get(this.playgroundApiUrl + playgroundId);
  }
}
