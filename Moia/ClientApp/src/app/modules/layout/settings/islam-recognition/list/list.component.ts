import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Observable } from 'rxjs';
import {  IsslamRecognition, IsslamRecognitionDto, SwaggerClient, ViewerPaginationOfIsslamRecognitionDto } from 'src/app/Shared/Services/Swagger/SwaggerClient.service';
import { EncryptDecryptService } from 'src/app/Shared/Services/encrypt-decrypt.service';
import { DefaultListComponent } from 'src/app/Shared/helpers/default-list.component';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent extends DefaultListComponent<ViewerPaginationOfIsslamRecognitionDto, IsslamRecognition> {

  display: boolean = false;
  model: IsslamRecognition;
  constructor(router: Router,
    private swagger: SwaggerClient,
    auth: EncryptDecryptService,
    confirmationService: ConfirmationService,
    toastr: MessageService) {
    super(router, auth, confirmationService, toastr);
    this.model = new IsslamRecognition()
  }

  showDialog() {
    this.display = true;
  }

  CancelDialog() {
    this.display = false;
  }

  returnDataFn(): Observable<ViewerPaginationOfIsslamRecognitionDto> {

    return this.swagger.apiIslamRecognitionWayGetAllCustomGet(
      this.searchTermControl.value,
      this.page,
      this.pageSize
    );
  }

  returnDeleteFn(id: number): Observable<any> {
    return this.swagger.apiIslamRecognitionWayDeletePost(id);
  }

  returnAddFn(): Observable<any> {
    return this.swagger.apiIslamRecognitionWayRecognitionWayPost(this.model);
  }


  async GetModelById(id: any) {
    try {
      const res = await this.swagger.apiIslamRecognitionWayGetByIdGet(id).toPromise();
      this.model = res?.toJSON()
      this.showDialog();
    } catch (error) {
      console.error('error in GetModelById:', error);
    }
  }


  addnew() {
    this.model = new IsslamRecognition()
    this.showDialog()
  }

  Add() {
    this.swagger.apiIslamRecognitionWayRecognitionWayPost({
      "id": this.model.id,
      "title": this.model.title,
    } as IsslamRecognition).subscribe(
      (response) => {
        this.getData();
        this.CancelDialog()
      },
      (err: HttpErrorResponse) => {

      }
    );
  }

 async Edit(id: any) {
    await  this.GetModelById(id)
    }
}

