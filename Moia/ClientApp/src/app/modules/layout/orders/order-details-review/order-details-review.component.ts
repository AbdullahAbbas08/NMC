
import { Component, OnInit } from '@angular/core';
import { IsslamRecognition, MaritalStatus, MuslimeDto, OrderStage, OrderStatus, SwaggerClient } from '../../../../Shared/Services/Swagger/SwaggerClient.service';
import { Location } from '@angular/common';
import { ConfirmationService } from 'primeng/api';
import { ActivatedRoute, Router } from '@angular/router';
import { EncryptDecryptService } from 'src/app/Shared/Services/encrypt-decrypt.service';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-order-details-review',
  templateUrl: './order-details-review.component.html',
  styleUrls: ['./order-details-review.component.scss']
})
export class OrderDetailsReviewComponent implements OnInit {
  url: any = this.route.snapshot.paramMap.get('id')
    ? this.Encryption.decryptUsingAES256(this.route.snapshot.paramMap.get('id')?.replace(/__/g, "/")) : 0
  // url: any = this.route.snapshot.paramMap.get('id')
  display: boolean = false;
  Muslime: MuslimeDto = new MuslimeDto()
  IslamRecognitionWays: IsslamRecognition[] = []
  Notes: string
  Manager: string = ''
  constructor(private swagger: SwaggerClient,
    private location: Location,
    private confirmationService: ConfirmationService,
    private route: ActivatedRoute,
    private Encryption: EncryptDecryptService,
    private translate: TranslateService,
    private router: Router,
    private authService: AuthService
  ) {
    this.RecognitionWays()
  }

  ngOnInit(): void {

    this.getMuslimeDetailsData()
  }

  getMuslimeDetailsData() {
    this.swagger.apiMuslimeGetDataGet(this.url).subscribe(
      res => {
        this.Muslime = res
      }
    )
  }

  Back() {
    this.location.back()
  }

  ConfirmOrder(OrderCode: any) {
    let userPermissions = this.authService.getUserPermissions();

    if (userPermissions.includes('CommitteeManager') ||
      userPermissions.includes('DepartmentManager') ||
      userPermissions.includes('NegoiatedDepartmentManager') ||
      userPermissions.includes('NegoiatedBranchManager') ||
      userPermissions.includes('BranchManager')) {



      if (userPermissions.includes('CommitteeManager')) {
        this.swagger.apiMuslimeChangeOrderStatePost(OrderCode,
          OrderStatus.Accept,
          OrderStage.Department,
          this.translate.instant('SendOrderToDepartmentDescription') + " " +
          " بواسطة " + this.authService.User$.value['Name']
        ).subscribe(res => {
          this.router.navigateByUrl('/order')
        })
      }

      else if (userPermissions.includes('DepartmentManager') || userPermissions.includes('NegoiatedDepartmentManager')) {
        this.swagger.apiMuslimeChangeOrderStatePost(OrderCode,
          OrderStatus.Accept,
          OrderStage.Branch,
          this.translate.instant('SendOrderToBranchDescription') + " " +
          " بواسطة " + this.authService.User$.value['Name']
        ).subscribe(res => {
          this.router.navigateByUrl('/order')
        })
      }

      else if (userPermissions.includes('BranchManager') || userPermissions.includes('NegoiatedBranchManager')) {
        this.swagger.apiMuslimeChangeOrderStatePost(OrderCode,
          OrderStatus.Finished,
          OrderStage.ReadyToPrintCard,
          this.translate.instant('ConfirmBranchManagerDescription')).subscribe(res => {
            this.router.navigateByUrl('/order')
          })
      }
    }
  }

  Confirm() {
    this.confirmationService.confirm({
      message: this.translate.instant('confirmordermessage'),
      key: 'SendConfirm',
      accept: () => {
        this.ConfirmOrder(this.url)
      },
    });
  }

  show() {
    this.display = true;
  }

  RefuseOrder() {
    let userPermissions = this.authService.getUserPermissions();
    if (userPermissions.includes('CommitteeManager')) {
      this.Manager = 'مدير الجمعية'
    } else if (userPermissions.includes('DepartmentManager')) {
      this.Manager = 'مدير إدارة الدعوة والإرشاد'
    } else if (userPermissions.includes('NegoiatedDepartmentManager')) {
      this.Manager = ' نائب عن مدير إدارة الدعوة والإرشاد'
    }
    else if (userPermissions.includes('BranchManager')) {
      this.Manager = 'مدير فرع الوزارة بالمنطقة'
    }
    else if (userPermissions.includes('NegoiatedBranchManager')) {
      this.Manager = ' نائب عن مدير فرع الوزارة بالمنطقة'
    }

    if (this.Notes && this.Notes.length > 10) {
      let _notes = `تم رفض الطلب من  ${this.Manager} للأسباب التالية : ${this.Notes}`
      this.swagger.apiMuslimeChangeOrderStatePost(this.url, OrderStatus.Reject, OrderStage.DataEntry, _notes).subscribe(
        res => {
          this.display = false
          this.Notes = ''
          this.router.navigateByUrl('order')
        }
      )
    }
  }

  close() {
    this.display = false;
    this.Notes = ''
  }

  RecognitionWays() {
    this.swagger.apiLookupRecognitionWaysGet().subscribe(
      res => {
        this.IslamRecognitionWays = res
      }
    )
  }

  recognitionWayExist(item: number): boolean {
    let res = this.Muslime.isslamRecognition?.islamRecognitionWay?.some(x => x.id == item) ?? false
    return res
  }

  getMaritalStatusString(status: any): string {
    switch (status) {
      case MaritalStatus.Single:
        return 'أعزب';
      case MaritalStatus.Married:
        return 'متزوج';
      case MaritalStatus.Divorced:
        return 'مطلق';
      case MaritalStatus.Widowed:
        return 'أرمل';
      default:
        return 'غير معروف';
    }
  }


}
