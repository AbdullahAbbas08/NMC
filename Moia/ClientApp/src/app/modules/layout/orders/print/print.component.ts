import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmationService, MessageService, PrimeIcons } from 'primeng/api';
import { Observable } from 'rxjs';
import { CommitteeIdName, OrderHistoryDto, OrderListDto, OrderStage, OrderStatus, SwaggerClient, ViewerPaginationOfOrderListDto } from 'src/app/Shared/Services/Swagger/SwaggerClient.service';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CustomApiService } from 'src/app/Shared/Services/custom-api.service';
import { EncryptDecryptService } from 'src/app/Shared/Services/encrypt-decrypt.service';
import { DefaultListComponent } from 'src/app/Shared/helpers/default-list.component';
import { environment } from 'src/environments/environment.prod';

@Component({
  selector: 'app-print',
  templateUrl: './print.component.html',
  styleUrls: ['./print.component.scss']
})
export class PrintComponent extends DefaultListComponent<ViewerPaginationOfOrderListDto, OrderListDto> {
  Committees: CommitteeIdName[] = []
  isChecked: boolean = false;
  Committee: CommitteeIdName;
  OrderCodes: string[] = []
  checkAll = true;
  events: any[] = [];
  display: boolean = false;
  pdfSrc: any = ''

  constructor(router: Router,
    private swagger: SwaggerClient,
    auth: EncryptDecryptService,
    confirmationService: ConfirmationService,
    private toastr: MessageService,
    private ApiService: CustomApiService,
    private translate: TranslateService,
    private authService: AuthService) {
    super(router, auth, confirmationService, toastr);
    this.getCommittees()

  }

  returnDataFn(): Observable<ViewerPaginationOfOrderListDto> {

    return this.swagger.apiCommitteeGetFinishedOrdersPost(
      this.searchTermControl.value,
      this.page,
      this.pageSize,
      this.Committee?.id,
      null
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

  PrintCard(code: any) {

    this.confirmationService.confirm({
      message: this.translate.instant('ConfirmPrintOperation'),
      key: 'ConfirmPrintOperation',
      accept: () => {
        this.isLoading = true;
        this.swagger.apiMuslimePrintCardGet(code).subscribe(
          res => {
            this.pdfSrc = res.file;
            const blob = this.base64toBlob(this.pdfSrc, 'application/pdf');
            const blobUrl = URL.createObjectURL(blob);
  
            const newWindow = window.open(blobUrl, '_blank');
            this.isLoading = false;
            if (newWindow) {
              newWindow.document.write('<iframe width="100%" height="100%" src="' + blobUrl + '"></iframe>');
            }
  
            this.swagger.apiMuslimeChangeOrderStatePost(code, OrderStatus.Printed, OrderStage.ReadyToPrintCard,
              this.translate.instant('ConfirmPrintDescription') + this.authService.User$.value['Name']).subscribe(
                res => {
                  // Handle the response if needed
                  // this.getData();
                }
              );
          }
        );
      },
    });

    // this.confirmationService.confirm({
    //   message: this.translate.instant("Areyousureyouwanttoprintcard"),
    //   header: 'Confirm Print',
    //   icon: 'pi pi-print',
    //   acceptLabel:this.translate.instant('print'),
    //   rejectLabel: this.translate.instant('cancel'),


    //   accept: () => {
    //     this.isLoading = true;
    //     this.swagger.apiMuslimePrintCardGet(code).subscribe(
    //       res => {
    //         this.pdfSrc = res.file;
    //         const blob = this.base64toBlob(this.pdfSrc, 'application/pdf');
    //         const blobUrl = URL.createObjectURL(blob);
  
    //         const newWindow = window.open(blobUrl, '_blank');
    //         this.isLoading = false;
    //         if (newWindow) {
    //           newWindow.document.write('<iframe width="100%" height="100%" src="' + blobUrl + '"></iframe>');
    //         }
  
    //         this.swagger.apiMuslimeChangeOrderStatePost(code, OrderStatus.Printed, OrderStage.ReadyToPrintCard,
    //           this.translate.instant('ConfirmPrintDescription') + this.authService.User$.value['Name']).subscribe(
    //             res => {
    //               // Handle the response if needed
    //               // this.getData();
    //             }
    //           );
    //       }
    //     );
    //   },
    //   reject: () => {
    //     // Handle the rejection case if needed
    //   }
    // });
  }

  AddCardFromTawakkalnaConfirm(code:any){

    this.confirmationService.confirm({
      message: this.translate.instant('AddCardIntoTawakklnaIntegration'),
      key: 'addCardIntoTawaklana',
      accept: () => {
        this.isLoading = true;
        this.AddCardFromTawakkalna(code)
      },
    });

    // this.confirmationService.confirm({
    //   message: this.translate.instant("Areyousureyouwanttoaddcardtotwakalna"),
    //   header: 'Confirm Print',
    //   icon: 'pi pi-print',
    //   acceptLabel:this.translate.instant('add'),
    //   rejectLabel: this.translate.instant('cancel'),
    // })
    
  }

  DeleteCardFromTawakkalna(orderCode: any) {
    this.confirmationService.confirm({
      message:this.translate.instant('deleteConfirm_card_integration'),
      key: 'deleteConfirm_card_integration',
      accept: () => {
        this.isLoading = true;
        this._DeleteCardFromTawakkalna(orderCode)
      },
    });
  }

  _DeleteCardFromTawakkalna(code: any) {
    this.swagger.apiMuslimeDeleteCardTawakkalnaPost(code).subscribe(
      res => {
        this.isLoading = true;
        if (res) {
          this.toast.add({
            severity: 'success',
            detail: this.translate.instant('success_DeleteCardTawakkalna'),
          });
        } else {
          this.toast.add({
            severity: 'error',
            detail: this.translate.instant('error_DeleteCardTawakkalna'),
          });
        }
        this.isLoading = false;
      },
      error => {
        this.isLoading = false;
        this.toast.add({
          severity: 'error',
          detail: this.translate.instant('error_DeleteCardTawakkalna'),
        });
      }
    );
  }

  UpdateCardFromTawakkalnaConfirm(code: any) {

    this.confirmationService.confirm({
      message: this.translate.instant('UpdateCardIntoTawakklnaIntegration'),
      key: 'updateCardIntoTawaklana',
      accept: () => {
        this.isLoading = true;
        this.UpdateCardFromTawakkalna(code)
      },
    });

   
  }


  UpdateCardFromTawakkalna(code: any) {
    this.swagger.apiMuslimeUpdateCardTawakkalnaPost(code).subscribe(
      res => {
        if (res) {
          this.toast.add({
            severity: 'success',
            detail: this.translate.instant('success_UpdateCardTawakkalna'),
          });
        } else {
          this.toast.add({
            severity: 'error',
            detail: this.translate.instant('error_UpdateCardTawakkalna'),
          });
        }
        this.isLoading = false;
      }
    )
  }


  AddCardFromTawakkalna(code: any) {
    this.isLoading = true;
    this.swagger.apiMuslimeAddTawakkalnaCardPost(code).subscribe(
      res => {
        if (res) {
          this.toast.add({
            severity: 'success',
            detail: this.translate.instant('success_UpdateCardTawakkalna'),
          });
        } else {
          this.toast.add({
            severity: 'error',
            detail: this.translate.instant('error_UpdateCardTawakkalna'),
          });
        }
        this.isLoading = false;
      }
    )
  }

  base64toBlob(base64Data: string, contentType: string): Blob {
    const sliceSize = 512;
    const byteCharacters = atob(base64Data);
    const byteArrays: Uint8Array[] = [];

    for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
      const slice = byteCharacters.slice(offset, offset + sliceSize);
      const byteNumbers = new Array(slice.length);

      for (let i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }

      byteArrays.push(new Uint8Array(byteNumbers));
    }

    return new Blob(byteArrays, { type: contentType });
  }


  showConfirmBtn(): boolean {
    return this.entities?.some(x => x.isChecked == true) ?? false
  }

  DataEntryChangeOrderState(code: string) {
    this.swagger.apiMuslimeChangeOrderStatePost(code, OrderStatus.Send, OrderStage.Committee,
      this.translate.instant('SendOrderToCommitteeDescription') + this.authService.User$.value['Name'] + " بواسطة ").subscribe(
        res => {
          this.getData()
        })
  }


}
