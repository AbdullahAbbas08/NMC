import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PresherListComponent } from './presher-list/presher-list.component';

const routes: Routes = [
  { path: "", component: PresherListComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PresherRoutingModule { }
