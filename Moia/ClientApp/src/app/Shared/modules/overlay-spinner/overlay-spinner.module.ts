import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OverlaySpinnerComponent } from './overlay-spinner.component';



@NgModule({
  declarations: [
    OverlaySpinnerComponent
  ],
  imports: [
    CommonModule,
  ],
  exports: [OverlaySpinnerComponent],
})
export class OverlaySpinnerModule { }
