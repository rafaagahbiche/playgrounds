import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { FileUploader } from 'ng2-file-upload';



@Injectable({
  providedIn: 'root'
})
export class PhotosService {

  constructor(private http: HttpClient) { }
  baseUrl = environment.apiUrl + 'member/photos';
  photoApiUrl = environment.apiUrl + 'photos';

  getMemberPhotos(token: any) {
    if (token !== null) {
      const httpOptions = {
        headers: new HttpHeaders({
          'Authorization': 'Bearer ' + token
        })
      };

      return this.http.get(this.baseUrl, httpOptions);
    }

    return null;
  }

  getRecentPhotos(count) {
    return this.http.get(this.photoApiUrl + '/recent?count=' + count);
  }

  createFileUploader(token) {
    return new FileUploader({
      url: this.baseUrl + '/upload/',
      authToken: 'Bearer ' + token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });
  }
}
