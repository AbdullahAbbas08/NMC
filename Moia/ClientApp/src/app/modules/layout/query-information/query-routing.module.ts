import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainQueryComponent } from './main-query/main-query.component';

const routes: Routes = [
  {
    path: 'details/:id', component: MainQueryComponent,
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class QueryRoutingModule { }
