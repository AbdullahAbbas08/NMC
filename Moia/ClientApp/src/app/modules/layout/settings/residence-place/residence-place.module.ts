import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ResidencePlaceRoutingModule } from './residence-place-routing.module';
import { ResidenceListComponent } from './residence-list/residence-list.component';
import { SharedModule } from 'src/app/Shared/shared.module';


@NgModule({
  declarations: [
    ResidenceListComponent
  ],
  imports: [
    CommonModule,
    ResidencePlaceRoutingModule,
    SharedModule
  ]
})
export class ResidencePlaceModule { }
