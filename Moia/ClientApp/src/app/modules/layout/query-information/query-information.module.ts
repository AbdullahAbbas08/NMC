import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { QueryRoutingModule } from './query-routing.module';
import { MainQueryComponent } from './main-query/main-query.component';
import { SharedModule } from 'src/app/Shared/shared.module';



@NgModule({
  declarations: [MainQueryComponent],
  imports: [
    CommonModule,
    QueryRoutingModule,
    SharedModule
  ]
})
export class QueryInformationModule { }
