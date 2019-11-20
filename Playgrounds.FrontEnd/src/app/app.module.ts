import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JwtModule } from '@auth0/angular-jwt';
import { RouterModule } from '@angular/router';
import { BsDatepickerModule } from 'ngx-bootstrap';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FileUploadModule } from 'ng2-file-upload';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { PhotosService } from './_services/photos.service';
import { PlaygroundsService } from './_services/playgrounds.service';
import { HomeComponent } from './home/home.component';
import { GalleryComponent } from './home/gallery/gallery.component';
import { LocationsComponent } from './home/locations/locations.component';
import { SingleGalleryPhotoComponent } from './home/gallery/single-gallery-photo/single-gallery-photo.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { PhotosComponent } from './member/photos/photos.component';
import { PhotoEditorComponent } from './member/photo-editor/photo-editor.component';
import { PhotosManagerComponent } from './member/photos-manager/photos-manager.component';
import { environment } from 'src/environments/environment';
import { appRoutes } from './routes';
import { FooterComponent } from './footer/footer.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { PlaygroundComponent } from './playground/playground.component';
import { PlaygroundDetailResolver } from './_resolvers/playground-detail.resolver';
import { PlaygroundListResolver } from './_resolvers/playground-list.resolver';

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
      PhotoEditorComponent,
      PhotosManagerComponent,
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
      BsDatepickerModule.forRoot(),
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
      ErrorInterceptorProvider
   ],
   bootstrap: [
      AppComponent
   ],
   schemas: []
})

export class AppModule { }
