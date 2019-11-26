import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JwtModule } from '@auth0/angular-jwt';
import { RouterModule } from '@angular/router';
import { BsDatepickerModule, TimepickerModule } from 'ngx-bootstrap';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FileUploadModule } from 'ng2-file-upload';
import {AutocompleteLibModule} from 'angular-ng-autocomplete';
 
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { PhotosService } from './_services/photos.service';
import { PlaygroundsService } from './_services/playgrounds.service';

import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';

import { PlaygroundComponent } from './playground/playground.component';
import { LocationsComponent } from './home/locations/locations.component';

import { GalleryComponent } from './home/gallery/gallery.component';
import { SingleGalleryPhotoComponent } from './home/gallery/single-gallery-photo/single-gallery-photo.component';
import { PhotosComponent } from './member/photos/photos.component';
import { PhotoUploaderComponent } from './member/photo-uploader/photo-uploader.component';
import { PhotosManagerComponent } from './member/photos-manager/photos-manager.component';
import { PhotoEditorComponent } from './member/photo-editor/photo-editor.component';


import { environment } from 'src/environments/environment';
import { appRoutes } from './routes';
import { FooterComponent } from './footer/footer.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { PlaygroundDetailResolver } from './_resolvers/playground-detail.resolver';
import { PlaygroundListResolver } from './_resolvers/playground-list.resolver';
import { PhotoEditorResolver } from './_resolvers/photo-editor.resolver';

export function tokenGetter() {
   return localStorage.getItem('token');
}

@NgModule({
   declarations: [
      AppComponent,
      PhotosComponent,
      NavComponent,
      LoginComponent,
      HomeComponent,
      GalleryComponent,
      LocationsComponent,
      SingleGalleryPhotoComponent,
      RegisterComponent,
      PhotoUploaderComponent,
      PhotosManagerComponent,
      PhotoEditorComponent,
      FooterComponent,
      PlaygroundComponent
   ],
   imports: [
      BrowserModule,
      BrowserAnimationsModule,
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      NgxSpinnerModule,
      FileUploadModule,
      AutocompleteLibModule,
      BsDatepickerModule.forRoot(),
      TimepickerModule.forRoot(),
      RouterModule.forRoot(appRoutes),
      JwtModule.forRoot({
         config:
         {
            tokenGetter: tokenGetter,
            whitelistedDomains: [environment.apiUrl],
            blacklistedRoutes: [environment.apiUrl + 'auth']
         }}
      )
   ],
   providers: [
      AuthService,
      PhotosService,
      PlaygroundsService,
      PlaygroundDetailResolver,
      PlaygroundListResolver,
      PhotoEditorResolver,
      ErrorInterceptorProvider
   ],
   bootstrap: [
      AppComponent
   ],
   schemas: []
})

export class AppModule { }
