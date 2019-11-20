import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './_guards/auth.guard';
import { PhotosManagerComponent } from './member/photos-manager/photos-manager.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { PlaygroundComponent } from './playground/playground.component';
import { PlaygroundDetailResolver } from './_resolvers/playground-detail.resolver';
import { LocationsComponent } from './home/locations/locations.component';
import { PlaygroundListResolver } from './_resolvers/playground-list.resolver';
import { PhotoEditorComponent } from './member/photo-editor/photo-editor.component';
import { PhotoEditorResolver } from './_resolvers/photo-editor.resolver';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'home', component: HomeComponent},
    { path: 'login', component: LoginComponent},
    { path: 'register', component: RegisterComponent},
    { path: 'playground/:id', component: PlaygroundComponent, resolve: {playground: PlaygroundDetailResolver}},
    { path: 'locations/:locationId', component: LocationsComponent, resolve: {playgrounds: PlaygroundListResolver}},
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'member/photos-manager', component: PhotosManagerComponent},
            { path: 'member/photo-editor/:id', component: PhotoEditorComponent, resolve: {photo: PhotoEditorResolver}}
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full'}
];
