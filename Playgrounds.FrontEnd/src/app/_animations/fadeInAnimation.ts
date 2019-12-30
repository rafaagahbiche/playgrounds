import { trigger, state, animate, transition, style } from '@angular/animations';

export const fadeInAnimation =
    trigger('fadeInAnimation', [
        state('in', style({ opacity: 1 })),
        transition(':enter', [
            style({ opacity: 0 }),
            animate('.8s ease-in-out')
        ])
    ]);
