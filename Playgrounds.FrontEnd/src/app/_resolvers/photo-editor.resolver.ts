import {Injectable} from '@angular/core';
import {Photo} from '../_models/Photo';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { PhotosService } from '../_services/photos.service';

@Injectable()
export class PhotoEditorResolver implements Resolve<Photo> {
    constructor(private router: Router, private photosService: PhotosService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Photo> {
        return this.photosService.getPhoto(route.params['id']).pipe(
            catchError(error => {
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
