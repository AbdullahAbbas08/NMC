import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ConfirmationService, MessageService, PrimeIcons } from 'primeng/api';
import { Observable } from 'rxjs';
import { CommitteeIdName, OrderListDto, OrderStage, OrderStatus, SwaggerClient, ViewerPaginationOfOrderListDto } from 'src/app/Shared/Services/Swagger/SwaggerClient.service';
import { CustomApiService } from 'src/app/Shared/Services/custom-api.service';
import { EncryptDecryptService } from 'src/app/Shared/Services/encrypt-decrypt.service';
import { DefaultListComponent } from 'src/app/Shared/helpers/default-list.component';

@Component({
  selector: 'app-reviewing',
  templateUrl: './reviewing.component.html',
  styleUrls: ['./reviewing.component.scss']
})
export class ReviewingComponent extends DefaultListComponent<ViewerPaginationOfOrderListDto, OrderListDto> {
  Committees: CommitteeIdName[] = []
  isChecked: boolean = false;
  Committee: CommitteeIdName;
  OrderCodes: string[] = []
  checkAll = true;
  events: any[];
  displayDialogRefuse: boolean = false;
  RefuseDiscription: any = '';
  displayOrderTimeLine: boolean = false;
  constructor(router: Router,
    private swagger: SwaggerClient,
    auth: EncryptDecryptService,
    confirmationService: ConfirmationService,
    toastr: MessageService,
    private ApiService: CustomApiService) {
    super(router, auth, confirmationService, toastr);
    this.getCommittees()

  }


  displayOrderTimeLineFn(Order: OrderListDto) {
    let values = []
    Order?.orderTimeLine && Order?.orderTimeLine.forEach(element => {
      values.push(
        { status: element.description, date: element.actionDate, icon: PrimeIcons.SHOPPING_CART, color: '#9C27B0' },
      );
    });

    this.RefuseDiscription = values.length > 0 ? Order?.orderTimeLine && Order?.orderTimeLine[values.length - 1].description : '';
    values.push({ status: `الطلب الأن للمراجعة عند  ${Order?.stage}  `, date: new Date().toLocaleString(), icon: PrimeIcons.COG, color: '#673AB7' })
    this.events = values
    this.displayOrderTimeLine = true
  }

  returnDataFn(): Observable<ViewerPaginationOfOrderListDto> {

    return this.swagger.apiCommitteeGetCommitteeOrdersPost(
      this.searchTermControl.value,
      this.page,
      this.pageSize,
      this.Committee?.id,
      null,
      [OrderStatus.Reject]
    );
  }

  getCommittees() {
    this.swagger.apiCommitteeGetAllCommitteesGet().subscribe(res => {
      this.Committees = res
    })
  }

  returnDeleteFn(id: number): Observable<any> {
    return this.swagger.apiWitnessDeletePost(id);
  }

  checkAllRows() {
    this.checkAll = !this.checkAll;
    if (this.checkAll) {
      this.OrderCodes = [];
      this.entities?.forEach(element => {
        element.isChecked = false;
      });
    }
    else {
      this.entities?.forEach(element => {
        element.isChecked = true;
        if (element.isChecked && element.orderCode) {
          this.updateOrderCodes(element.orderCode, true);
        }
      });
    }
  }


  updateOrderCodes(code: string, isChecked: boolean) {
    if (isChecked) {
      if (code && !this.OrderCodes.includes(code)) {
        this.OrderCodes.push(code);
      }
    } else {
      const index = this.OrderCodes.indexOf(code);
      if (index !== -1) {
        this.OrderCodes.splice(index, 1);
      }
    }
  }

  addToOrderCodes(code: string) {
    if (!this.OrderCodes.includes(code)) {
      this.OrderCodes.push(code);
    }
  }

  updateCheckedStatus(code: string, event: any) {
    if (event.target.checked) {
      this.addToOrderCodes(code);
    } else {
      const index = this.OrderCodes.indexOf(code);
      if (index !== -1) {
        this.OrderCodes.splice(index, 1);
      }
    }
  }

  RefuseDiscriptionFn(entity: OrderListDto) {
    // console.log(this.RefuseDiscription);
    this.RefuseDiscription = entity.orderTimeLine?.slice(-1)[0].description
    this.displayDialogRefuse = true;
  }







  showConfirmBtn(): boolean {
    return this.entities?.some(x => x.isChecked == true) ?? false
  }

  orderCodesRe() {
  }

  Edit(muslimeId: number) {
    this.router.navigate(['muslime/personal-data', this.auth.encryptUsingAES256(muslimeId)])
  }


}
