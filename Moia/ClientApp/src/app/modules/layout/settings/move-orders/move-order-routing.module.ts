import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MoveOrdersComponent } from './move-orders.component';

const routes: Routes = [
  { path: "", component: MoveOrdersComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MoveOrdersRoutingModule { }
