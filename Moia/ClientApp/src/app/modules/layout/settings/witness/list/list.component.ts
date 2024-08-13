import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Observable } from 'rxjs';
import { SwaggerClient, ViewerPaginationOfWitness, Witness } from 'src/app/Shared/Services/Swagger/SwaggerClient.service';
import { EncryptDecryptService } from 'src/app/Shared/Services/encrypt-decrypt.service';
import { DefaultListComponent } from 'src/app/Shared/helpers/default-list.component';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent extends DefaultListComponent<ViewerPaginationOfWitness, Witness> {

  display: boolean = false;
  model: Witness;
  constructor(router: Router,
    private swagger: SwaggerClient,
    auth: EncryptDecryptService,
    confirmationService: ConfirmationService,
    toastr: MessageService) {
    super(router, auth, confirmationService, toastr);
    this.model = new Witness()
  }

  showDialog() {
    this.display = true;
  }

  CancelDialog() {
    this.display = false;
  }

  returnDataFn(): Observable<ViewerPaginationOfWitness> {

    return this.swagger.apiWitnessGetAllCustomGet(
      this.searchTermControl.value,
      this.page,
      this.pageSize
    );
  }

  returnDeleteFn(id: number): Observable<any> {
    return this.swagger.apiWitnessDeletePost(id);
  }

  returnAddFn(): Observable<any> {
    return this.swagger.apiWitnessInsertPost(this.model);
  }


  async GetModelById(id: any) {
    try {
      const res = await this.swagger.apiWitnessGetByIdGet(id).toPromise();
      this.model = res?.toJSON()
      this.showDialog();
    } catch (error) {
      console.error('error in GetModelById:', error);
    }
  }


  addnew() {
    this.model = new Witness()
    this.showDialog()
  }

  Add() {

    this.swagger.apiWitnessInsertPost(this.model).subscribe(
      (response) => {
        if (response.data != null) {
          this.toast.add({
            severity: 'success',
            detail: response.message,
          });

          this.getData();
          this.CancelDialog()
        }
        else {
          this.toast.add({
            severity: 'error',
            detail: response.message,
          });
        }

      },
      (err: HttpErrorResponse) => {

      }
    );
  }

  async Edit(id: any) {
    await this.GetModelById(id)
    return this.swagger.apiWitnessInsertPost(
      this.model
    );
  }
}
