import {Injectable} from '@angular/core';
import {Playground} from '../_models/Playground';
import { PlaygroundsService } from '../_services/playgrounds.service';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class PlaygroundListResolver implements Resolve<Playground[]> {
    constructor(private router: Router, private playgroundService: PlaygroundsService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Playground[]> {
        return this.playgroundService.getPlaygroundsByLocationId(route.params['locationId']).pipe(
            catchError(error => {
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
