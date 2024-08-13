import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Observable } from 'rxjs';
import { Preacher, SwaggerClient, ViewerPaginationOfPreacher } from 'src/app/Shared/Services/Swagger/SwaggerClient.service';
import { EncryptDecryptService } from 'src/app/Shared/Services/encrypt-decrypt.service';
import { DefaultListComponent } from 'src/app/Shared/helpers/default-list.component';

@Component({
  selector: 'app-presher-list',
  templateUrl: './presher-list.component.html',
  styleUrls: ['./presher-list.component.scss']
})
export class PresherListComponent extends DefaultListComponent<ViewerPaginationOfPreacher, Preacher> {

  display: boolean = false;
  model: any
  manager: Preacher
  SelectEdentity: Preacher = {
    title: ''
  } as Preacher
  UsersList: Preacher[]

  constructor(router: Router,
    private swagger: SwaggerClient,
    auth: EncryptDecryptService,
    confirmationService: ConfirmationService,
    toastr: MessageService) {
    super(router, auth, confirmationService, toastr);
  }

  returnDataFn(): Observable<ViewerPaginationOfPreacher> {
    return this.swagger.apiLookupGetPreshersPaginatedGet(
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

  AddNew() {
    this.SelectEdentity = {
      title: ''
    } as Preacher
    this.display = true;
  }

  CancelDialog() {
    this.display = false;
  }

  Edit(entity: Preacher) {
    this.SelectEdentity = entity
    this.model = entity.title
    this.showDialog()
  }


  Add() {
    if (this.SelectEdentity.title != '') {
      this.swagger.apiLookupSavePresherPost(this.SelectEdentity).subscribe(
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
