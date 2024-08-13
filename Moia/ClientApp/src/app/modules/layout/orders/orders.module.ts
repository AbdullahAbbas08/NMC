import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersRoutingModule } from './orders-routing.module';
import { IndexComponent } from './index.component';
import { DisplayComponent } from './display/display.component';
import { WaitingComponent } from './waiting/waiting.component';
import { ReviewingComponent } from './reviewing/reviewing.component';
import { PrintComponent } from './print/print.component';
import { SharedModule } from '../../../Shared/shared.module';
import { OrderDetailsComponent } from './order-details/order-details.component';
import { PdfViewerModule } from 'ng2-pdf-viewer';
import {NgxPrintModule} from 'ngx-print';
import { AllMyOrdersComponent } from './all-my-orders/all-my-orders.component';
import { OrderDetailsReviewComponent } from './order-details-review/order-details-review.component';
import { ConfirmDialogModule } from 'primeng/confirmdialog';

@NgModule({
  declarations: [
    IndexComponent,
    DisplayComponent,
    WaitingComponent,
    ReviewingComponent,
    PrintComponent,
    OrderDetailsComponent,
    AllMyOrdersComponent,
    OrderDetailsReviewComponent
  ],
  imports: [
    CommonModule,
    OrdersRoutingModule,
    PdfViewerModule,
    SharedModule,
    NgxPrintModule,
    ConfirmDialogModule
  ]
})
export class OrdersModule { }
