import { NgModule } from '@angular/core';
import { NoAuthGuard } from './Shared/guards/no-auth.guard';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './Shared/guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import('./modules/layout/layout.module').then((m) => m.LayoutModule),
    canActivate:[AuthGuard]
  },
  {
    path: 'login',
    loadChildren: () =>
      import('./modules/_Authentication/auth.module').then((m) => m.AuthModule),
    canActivate: [NoAuthGuard],
  },
  {
    path: 'query-info',
    loadChildren: () =>
      import('../app/modules/layout/query-information/query-information.module').then((m) => m.QueryInformationModule),
    // canActivate: [BranchDataEntryGuard]
  },
  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full',
  },
];

// @NgModule({
//   imports: [RouterModule.forRoot(routes)],
//   exports: [RouterModule]
// })
@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
