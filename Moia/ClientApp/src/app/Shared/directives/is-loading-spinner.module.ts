import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IsLoadingSpinnerDirective } from './is-loading-spinner.directive';



@NgModule({
  imports: [CommonModule],
  declarations: [IsLoadingSpinnerDirective],
  exports: [IsLoadingSpinnerDirective],
})
export class IsLoadingSpinnerModule { }
