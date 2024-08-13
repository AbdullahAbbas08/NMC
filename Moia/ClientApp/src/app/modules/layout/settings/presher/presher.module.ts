import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PresherRoutingModule } from './presher-routing.module';
import { PresherListComponent } from './presher-list/presher-list.component';
import { SharedModule } from 'src/app/Shared/shared.module';


@NgModule({
  declarations: [
    PresherListComponent
  ],
  imports: [
    CommonModule,
    PresherRoutingModule,
    SharedModule
  ]
})
export class PresherModule { }
