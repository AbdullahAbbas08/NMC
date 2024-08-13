import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LocalizationListComponent } from './localization-list/localization-list.component';
import { LocalizationRoutingModule } from './localization-routing.module';
import { SharedModule } from 'src/app/Shared/shared.module';



@NgModule({
  declarations: [LocalizationListComponent],
  imports: [
    CommonModule,
    LocalizationRoutingModule,
    SharedModule
  ]
})
export class LocalizationModule { }
