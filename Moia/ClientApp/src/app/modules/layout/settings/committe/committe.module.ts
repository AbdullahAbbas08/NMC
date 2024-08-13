import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CommitteRoutingModule } from './committe-routing.module';
import { CommitteeListComponent } from './committee-list/committee-list.component';
import { SharedModule } from '../../../../Shared/shared.module';


@NgModule({
  declarations: [
    CommitteeListComponent
  ],
  imports: [
    CommonModule,
    CommitteRoutingModule,
    SharedModule
  ]
})
export class CommitteModule { }
