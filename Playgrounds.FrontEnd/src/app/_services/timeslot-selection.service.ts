import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TimeslotSelectionService {
  private timeslotSelected = new BehaviorSubject<string>('');
  currentTimeslotSelected = this.timeslotSelected.asObservable();
  private pixelsToTranslate = new BehaviorSubject<number>(0);
  currentPixelsToTranslate = this.pixelsToTranslate.asObservable();
  
  constructor() { }

  public setTimeslotSelection(selectedTime: string) {
    this.timeslotSelected.next(selectedTime);
  }

  public setPixelsToTranslate(pixelsLength: number) {
    this.pixelsToTranslate.next(pixelsLength);
  }
}
