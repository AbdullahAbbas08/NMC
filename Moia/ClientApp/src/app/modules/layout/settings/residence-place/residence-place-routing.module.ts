import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ResidenceListComponent } from './residence-list/residence-list.component';

const routes: Routes = [
  { path: "", component: ResidenceListComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ResidencePlaceRoutingModule { }
