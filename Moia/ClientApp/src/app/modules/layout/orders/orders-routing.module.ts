import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DisplayComponent } from './display/display.component';
import { WaitingComponent } from './waiting/waiting.component';
import { ReviewingComponent } from './reviewing/reviewing.component';
import { PrintComponent } from './print/print.component';
import { IndexComponent } from './index.component';
import { OrderDetailsComponent } from './order-details/order-details.component';
import { DataEntryGuard } from 'src/app/Shared/guards/data-entry.guard';
import { AllMyOrdersComponent } from './all-my-orders/all-my-orders.component';
import { OrderDetailsReviewComponent } from './order-details-review/order-details-review.component';
import { CanPrintGuard } from 'src/app/Shared/guards/can-print.guard';
import { CanReviewOrderGuard } from 'src/app/Shared/guards/can-review-order.guard';

const routes: Routes = [
  {
    path: '', component: IndexComponent,
    children: [
      { path: '', component: DisplayComponent, pathMatch: 'full' },//, canActivate: [ReviewOrderGuard]
      { path: 'preview-orders', component: AllMyOrdersComponent, pathMatch: 'full' },
      { path: 'waiting', component: WaitingComponent, pathMatch: 'full', canActivate: [DataEntryGuard] },
      { path: 'Details/:id', component: OrderDetailsComponent, pathMatch: 'full' },
      { path: 'preview-orders-review/:id', component: OrderDetailsReviewComponent, pathMatch: 'full' },
      { path: 'review', component: ReviewingComponent, pathMatch: 'full', canActivate: [CanReviewOrderGuard] },
      { path: 'print', component: PrintComponent, pathMatch: 'full', canActivate: [CanPrintGuard] },
    ]
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrdersRoutingModule { }
