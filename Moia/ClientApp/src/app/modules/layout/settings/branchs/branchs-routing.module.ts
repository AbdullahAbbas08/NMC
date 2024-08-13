import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BranchListComponent } from './branch-list/branch-list.component';
import { BranchFormComponent } from './branch-form/branch-form.component';

const routes: Routes = [
  { path: "", component: BranchListComponent },
  { path: "form", component: BranchFormComponent },
  { path: "form/:id", component: BranchFormComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BranchsRoutingModule { }
