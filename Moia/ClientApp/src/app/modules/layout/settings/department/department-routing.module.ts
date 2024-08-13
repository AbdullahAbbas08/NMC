import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DepartmentListComponent } from './department-list/department-list.component';
import { DepartmentFormComponent } from './department-form/department-form.component';

const routes: Routes = [
  { path: "", component: DepartmentListComponent },
  { path: "list", component: DepartmentListComponent },
  { path: "form", component: DepartmentFormComponent },
  { path: "form/:id", component: DepartmentFormComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DepartmentRoutingModule { }
