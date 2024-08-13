import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MoveOrdersComponent } from './move-orders.component';
import { MoveOrdersRoutingModule } from './move-order-routing.module';
import { SharedModule } from 'src/app/Shared/shared.module';

@NgModule({
  imports: [
    CommonModule,
    MoveOrdersRoutingModule,
    SharedModule
  ],
  declarations: [MoveOrdersComponent]
})
export class MoveOrdersModule { }
