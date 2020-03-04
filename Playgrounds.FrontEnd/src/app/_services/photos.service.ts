import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { FileUploader } from 'ng2-file-upload';
import { Photo } from '../_models/Photo';
import Utils from './utils';

@Injectable({
  providedIn: 'root'
})

export class PhotosService {

  private memberPhotosApiUrl = environment.apiUrl + 'member-photos';
  private photoApiUrl = environment.apiUrl + 'photos';

  constructor(private http: HttpClient) { }

  private getHttpOptions(token: any) {
    if (token !== null) {
      let httpOptions = Utils.getHttpOptionsHeader(token);
      return httpOptions;
    }
  }

  public getMemberPhotos(token: any) {
    if (token !== null) {
      let httpOptions = Utils.getHttpOptionsHeader(token);
      return this.http.get(this.memberPhotosApiUrl, httpOptions);
    }

    return null;
  }

  public getRecentPhotos(count: number) {
    return this.http.get(this.photoApiUrl + '/recent?count=' + count);
  }

  public getPlaygroundPhotos(playgroundId: number) {
    return this.http.get(this.photoApiUrl + '/playgrounds/' + playgroundId);
  }

  public getPlaygroundPosts(playgroundId: number) {
    return this.http.get(this.photoApiUrl + '/playgrounds/' + playgroundId + '/posts');
  }

  public getPhoto(id: number) {
    return this.http.get(this.photoApiUrl + '/' + id);
  }

  public deletePhoto(publicId: string, token: any) {
      return this.http.put(this.memberPhotosApiUrl + '/mark-as-deleted/' + publicId, this.getHttpOptions(token));
  }

  public updatePhoto(photo: Photo, token: any) {
    return this.http.put(this.memberPhotosApiUrl, photo, this.getHttpOptions(token));
  }

  public createFileUploader(token: any) {
    return new FileUploader({
      url: this.memberPhotosApiUrl,
      authToken: 'Bearer ' + token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });
  }
}
