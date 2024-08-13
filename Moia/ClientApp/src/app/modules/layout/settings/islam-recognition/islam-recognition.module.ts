import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { IslamRecognitionRoutingModule } from './islam-recognition-routing.module';
import { ListComponent } from './list/list.component';
import { FormComponent } from './form/form.component';
import { SharedModule } from 'src/app/Shared/shared.module';


@NgModule({
  declarations: [
    ListComponent,
    FormComponent
  ],
  imports: [
    CommonModule,
    IslamRecognitionRoutingModule,
    SharedModule
  ]
})
export class IslamRecognitionModule { }
