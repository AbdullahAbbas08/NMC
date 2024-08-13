import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { WitnessRoutingModule } from './witness-routing.module';
import { ListComponent } from './list/list.component';
import { WitnessFormComponent } from './witness-form/witness-form.component';
import { SharedModule } from '../../../../Shared/shared.module';


@NgModule({
  declarations: [
    ListComponent,
    WitnessFormComponent
  ],
  imports: [
    CommonModule,
    WitnessRoutingModule,
    SharedModule
  ]
})
export class WitnessModule { }
