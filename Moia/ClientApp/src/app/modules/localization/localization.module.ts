import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LocalizationRoutingModule } from './localization-routing.module';
import { LocalizationFormComponent } from './localization-form/localization-form.component';
import { LocalizationListComponent } from './localization-list/localization-list.component';


@NgModule({
  declarations: [
    LocalizationFormComponent,
    LocalizationListComponent
  ],
  imports: [
    CommonModule,
    LocalizationRoutingModule
  ]
})
export class LocalizationModule { }
