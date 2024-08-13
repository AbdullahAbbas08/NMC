import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BranchsRoutingModule } from './branchs-routing.module';
import { BranchListComponent } from './branch-list/branch-list.component';
import { BranchFormComponent } from './branch-form/branch-form.component';
import { SharedModule } from 'src/app/Shared/shared.module';


@NgModule({
  declarations: [
    BranchListComponent,
    BranchFormComponent,
  ],
  imports: [
    CommonModule,
    BranchsRoutingModule,
    SharedModule
  ]
})
export class BranchsModule { }
