import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './layout.component';
import { SettingsComponent } from './settings/settings.component';
import { ProfileComponent } from './profile/profile.component';
import { BranchDataEntryGuard } from 'src/app/Shared/guards/branch-data-entry.guard';
import { DataEntryGuard } from 'src/app/Shared/guards/data-entry.guard';
import { OrderGuard } from 'src/app/Shared/guards/order.guard';
import { CanMoveOrdersGuard } from 'src/app/Shared/guards/can-move-orders.guard';
import { ReportsGuard } from 'src/app/Shared/guards/reports.guard';

const routes: Routes = [

  {
    path: '',
    component: LayoutComponent,
    children: [

      {
        path: '', pathMatch: 'full',
        loadChildren: () =>
          import('./orders/orders.module').then((m) => m.OrdersModule),
        // import('./main/main.module').then((m) => m.MainModule),
      },
      {
        path: 'muslime',
        loadChildren: () =>
          import('./muslime/muslime.module').then((m) => m.MuslimeModule),
        canActivate: [DataEntryGuard]
      },
      {
        path: 'order',
        loadChildren: () =>
          import('./orders/orders.module').then((m) => m.OrdersModule),
        canActivate: [OrderGuard]
      },
      {
        path: 'islam-recognition',
        loadChildren: () =>
          import('./settings/islam-recognition/islam-recognition.module').then((m) => m.IslamRecognitionModule),
        canActivate: [BranchDataEntryGuard]
      },
      {
        path: 'witness',
        loadChildren: () =>
          import('./settings/witness/witness.module').then((m) => m.WitnessModule),
        canActivate: [BranchDataEntryGuard]
      },
      {
        path: 'committe',
        loadChildren: () =>
          import('./settings/committe/committe.module').then((m) => m.CommitteModule),
        canActivate: [BranchDataEntryGuard]
      },
      {
        path: 'user',
        loadChildren: () =>
          import('./settings/user/user.module').then((m) => m.UserModule),
        canActivate: [BranchDataEntryGuard]
      },
      {
        path: 'department',
        loadChildren: () =>
          import('./settings/department/department.module').then((m) => m.DepartmentModule),
        canActivate: [BranchDataEntryGuard]
      },
      {
        path: 'Presher',
        loadChildren: () =>
          import('./settings/presher/presher.module').then((m) => m.PresherModule),
        canActivate: [BranchDataEntryGuard]
      },
      {
        path: 'localization',
        loadChildren: () =>
          import('./settings/localization/localization.module').then((m) => m.LocalizationModule),
        canActivate: [BranchDataEntryGuard]
      },
      {
        path: 'ResidencePlace',
        loadChildren: () =>
          import('./settings/residence-place/residence-place.module').then((m) => m.ResidencePlaceModule),
        canActivate: [BranchDataEntryGuard]
      },
      {
        path: 'MoveOrders',
        loadChildren: () =>
          import('./settings/move-orders/move-orders.module').then((m) => m.MoveOrdersModule),
        canActivate: [CanMoveOrdersGuard]
      },
      {
        path: 'reports',
        loadChildren: () =>
          import('./settings/reports/reports.module').then((m) => m.ReportsModule),
        canActivate: [ReportsGuard]
      },
      {
        path: 'branch',
        loadChildren: () =>
          import('./settings/branchs/branchs.module').then((m) => m.BranchsModule),
        canActivate: [BranchDataEntryGuard]
      },
      { path: 'Setting', component: SettingsComponent, pathMatch: 'full' },
      { path: 'Profile', component: ProfileComponent, pathMatch: 'full' },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LayoutRoutingModule { }
