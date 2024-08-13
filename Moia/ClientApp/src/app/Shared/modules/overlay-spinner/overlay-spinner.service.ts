import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class OverlaySpinnerService {
  showCount: number = 0;
  hideCount: number = 0;
  constructor() {}

  show() {
    this.showCount++;
  }
  hide() {
    this.hideCount++;
  }
}
