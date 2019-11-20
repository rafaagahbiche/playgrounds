import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { FileUploader } from 'ng2-file-upload';

@Injectable({
  providedIn: 'root'
})

export class PhotosService {

  constructor(private http: HttpClient) { }
  memberApiUrl = environment.apiUrl + 'member';
  memberPhotosApiUrl = environment.apiUrl + 'member/photos';
  photoApiUrl = environment.apiUrl + 'photos';

  getMemberPhotos(token: any) {
    if (token !== null) {
      const httpOptions = {
        headers: new HttpHeaders({
          'Authorization': 'Bearer ' + token
        })
      };

      return this.http.get(this.memberPhotosApiUrl, httpOptions);
    }

    return null;
  }

  getRecentPhotos(count) {
    return this.http.get(this.photoApiUrl + '/recent?count=' + count);
  }

  deltePhoto(publicId: string, token: any) {
      return this.http.delete(this.memberPhotosApiUrl + '/' + publicId, this.getHttpOptions(token));
  }

  private getHttpOptions(token: any) {
    if (token !== null) {
      const httpOptions = {
        headers: new HttpHeaders({
          'Authorization': 'Bearer ' + token
        })
      };

      return httpOptions;
    }
  }

  createFileUploader(token) {
    return new FileUploader({
      url: this.memberPhotosApiUrl + '/upload/',
      authToken: 'Bearer ' + token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });
  }
}
