import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmationService, MessageService, PrimeIcons } from 'primeng/api';
import { Observable } from 'rxjs';
import {
  CommitteeIdName,
  OrderHistoryDto,
  OrderListDto,
  OrderStage,
  OrderStatus,
  SwaggerClient,
  ViewerPaginationOfOrderListDto,
} from 'src/app/Shared/Services/Swagger/SwaggerClient.service';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CustomApiService } from 'src/app/Shared/Services/custom-api.service';
import { EncryptDecryptService } from 'src/app/Shared/Services/encrypt-decrypt.service';
import { DefaultListComponent } from 'src/app/Shared/helpers/default-list.component';

@Component({
  selector: 'app-display',
  templateUrl: './display.component.html',
  styleUrls: ['./display.component.scss'],
})
export class DisplayComponent extends DefaultListComponent<
  ViewerPaginationOfOrderListDto,
  OrderListDto
> {
  Committees: CommitteeIdName[] = [];
  isChecked: boolean = false;
  Committee: CommitteeIdName;
  OrderCodes: string[] = [];
  checkAll = true;
  events: any[] = [];
  displayOrderTimeLine: boolean = false;
 inprogress:boolean=false
  constructor(
    router: Router,
    private swagger: SwaggerClient,
    auth: EncryptDecryptService,
    confirmationService: ConfirmationService,
    toastr: MessageService,
    private ApiService: CustomApiService,
    private translate: TranslateService,
    private authService: AuthService
  ) {
    super(router, auth, confirmationService, toastr);
    this.getCommittees();
  }

  // { status: 'Processing', date: '15/10/2020 14:00', icon: PrimeIcons.COG, color: '#673AB7' },
  // { status: 'Shipped', date: '15/10/2020 16:15', icon: PrimeIcons.ENVELOPE, color: '#FF9800' },
  // { status: 'Delivered', date: '16/10/2020 10:00', icon: PrimeIcons.CHECK, color: '#607D8B' }
  displayOrderTimeLineFn(Order: OrderListDto) {
    let values = [];
    Order?.orderTimeLine &&
      Order?.orderTimeLine.forEach((element) => {
        values.push({
          status: element.description,
          date: element.actionDate,
          icon: PrimeIcons.SHOPPING_CART,
          color: '#9C27B0',
        });
      });
    if (Order?.stage)
      values.push({
        status: `الطلب الأن للمراجعة عند  ${Order?.stage}  `,
        date: new Date().toLocaleString(),
        icon: PrimeIcons.COG,
        color: '#673AB7',
      });
    this.events = values;
    this.displayOrderTimeLine = true;
  }

  returnDataFn(): Observable<ViewerPaginationOfOrderListDto> {
    return this.swagger.apiCommitteeGetCommitteeOrdersPost(
      this.searchTermControl.value,
      this.page,
      this.pageSize,
      this.Committee?.id,
      null,
      [OrderStatus.Accept, OrderStatus.Create, OrderStatus.Send]
    );
  }

  getCommittees() {
    this.swagger.apiCommitteeGetAllCommitteesGet().subscribe((res) => {
      this.Committees = res;
    });
  }

  returnDeleteFn(id: number): Observable<any> {
    return this.swagger.apiWitnessDeletePost(id);
  }

  checkAllRows() {
    this.checkAll = !this.checkAll;
    if (this.checkAll) {
      this.OrderCodes = [];
      this.entities?.forEach((element) => {
        element.isChecked = false;
      });
    } else {
      this.entities?.forEach((element) => {
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

  Edit(muslimeId: number) {
    this.router.navigate([
      'muslime/personal-data',
      this.auth.encryptUsingAES256(muslimeId),
    ]);
  }

  showConfirmBtn(): boolean {
    return this.entities?.some((x) => x.isChecked == true) ?? false;
  }

  toggleConfirm() {
    this.isChecked = !this.isChecked;
  }

  display: boolean = false;

  OpenDialog(){
    this.display = true;
  }
  isLoadingdescision:boolean=false
  orderCodesRe() {
    this.isLoadingdescision=true
    this.inprogress = true;
    let userPermissions = this.authService.getUserPermissions();
    if (
      userPermissions.includes('CommitteeManager') ||
      userPermissions.includes('DepartmentManager') ||
      userPermissions.includes('NegoiatedDepartmentManager') ||
      userPermissions.includes('NegoiatedBranchManager') ||
      userPermissions.includes('BranchManager')
    ) {
      if (userPermissions.includes('CommitteeManager')) {
        this.OrderCodes.forEach((element) => {
          this.swagger
            .apiMuslimeChangeOrderStatePost(
              element,
              OrderStatus.Accept,
              OrderStage.Department,
              ''
              // this.translate.instant('SendOrderToDepartmentDescription') + " بواسطة " +
              // this.authService.User$.value['Name']
            )
            .subscribe((res) => {
              this.Cancel()
              this.getData();
            });
        });
      } else if (
        userPermissions.includes('DepartmentManager') ||
        userPermissions.includes('NegoiatedDepartmentManager')
      ) {
        this.OrderCodes.forEach((element) => {
          this.swagger
            .apiMuslimeChangeOrderStatePost(
              element,
              OrderStatus.Accept,
              OrderStage.Branch,
              ''
              // this.translate.instant('SendOrderToBranchDescription') + " بواسطة " + this.authService.User$.value['Name']
            )
            .subscribe((res) => {
              this.Cancel()
              this.getData();
            });
        });
      } else if (
        userPermissions.includes('BranchManager') ||
        userPermissions.includes('NegoiatedBranchManager')
      ) {
        this.OrderCodes.forEach((element) => {
          this.swagger
            .apiMuslimeChangeOrderStatePost(
              element,
              OrderStatus.Accept,
              OrderStage.ReadyToPrintCard,
              ''
              // this.translate.instant('ConfirmBranchManagerDescription')
            )
            .subscribe((res) => {
              this.Cancel()
              this.getData();
            });
        });
      }
    }
  }

  Cancel() {
    this.display = false;
    this.isChecked=false
    this.inprogress = false;
    this.isLoadingdescision = false;
  }

  DataEntryChangeOrderState(code: string) {
    //this.translate.instant('SendOrderToCommitteeDescription') + " بواسطة " + this.authService.User$.value['Name']
    this.swagger
      .apiMuslimeChangeOrderStatePost(
        code,
        OrderStatus.Send,
        OrderStage.Committee,
        ''
      )
      .subscribe((res) => {
        this.getData();
      });
  }
}
