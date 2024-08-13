import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Observable } from 'rxjs';
import { Preacher, ResidenceIssuePlace, SwaggerClient, ViewerPaginationOfPreacher, ViewerPaginationOfResidenceIssuePlace } from 'src/app/Shared/Services/Swagger/SwaggerClient.service';
import { EncryptDecryptService } from 'src/app/Shared/Services/encrypt-decrypt.service';
import { DefaultListComponent } from 'src/app/Shared/helpers/default-list.component';


@Component({
  selector: 'app-residence-list',
  templateUrl: './residence-list.component.html',
  styleUrls: ['./residence-list.component.scss']
})
export class ResidenceListComponent extends DefaultListComponent<ViewerPaginationOfResidenceIssuePlace, ResidenceIssuePlace> {

  display: boolean = false;
  model: any
  SelectEdentity: ResidenceIssuePlace = {
    title: ''
  } as Preacher

  constructor(router: Router,
    private swagger: SwaggerClient,
    auth: EncryptDecryptService,
    confirmationService: ConfirmationService,
    toastr: MessageService) {
    super(router, auth, confirmationService, toastr);
  }

  returnDataFn(): Observable<ViewerPaginationOfResidenceIssuePlace> {
    return this.swagger.apiLookupGetResidencePalcePaginatedGet(
      this.searchTermControl.value,
      this.page,
      this.pageSize
    );
  }

  returnDeleteFn(id: number): Observable<any> {
    return this.swagger.apiWitnessDeletePost(0);
  }

  showDialog() {
    this.display = true;
  }

  CancelDialog() {
    this.display = false;
  }

  Edit(entity: ResidenceIssuePlace) {
    this.SelectEdentity = entity
    this.showDialog()
  }


  Add() {
    if (this.SelectEdentity.title != '') {
      this.swagger.apiLookupSaveResidenceIssuePlacePost(this.SelectEdentity).subscribe(
        (response) => {
          this.getData()
          this.CancelDialog()
          this.SelectEdentity = {
            title: ''
          } as Preacher
        },
        (err: HttpErrorResponse) => {
        }
      );
    }
  }


}
