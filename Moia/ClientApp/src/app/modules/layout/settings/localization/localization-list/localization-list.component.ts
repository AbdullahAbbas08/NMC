import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Observable } from 'rxjs';
import { LocalizationDetailsDTO, Preacher, SwaggerClient, ViewerPaginationOfLocalizationDetailsDTO, ViewerPaginationOfPreacher } from 'src/app/Shared/Services/Swagger/SwaggerClient.service';
import { EncryptDecryptService } from 'src/app/Shared/Services/encrypt-decrypt.service';
import { DefaultListComponent } from 'src/app/Shared/helpers/default-list.component';

@Component({
  selector: 'app-localization-list',
  templateUrl: './localization-list.component.html',
  styleUrls: ['./localization-list.component.scss']
})
export class LocalizationListComponent extends DefaultListComponent<ViewerPaginationOfLocalizationDetailsDTO, LocalizationDetailsDTO> {

  display: boolean = false;
  model: any
  manager: Preacher
  SelectEdentity: LocalizationDetailsDTO = {
    id:0,
    key: '',
    valueAr: '',
    valueEn: '',
  } as LocalizationDetailsDTO
  UsersList: LocalizationDetailsDTO[]

  constructor(router: Router,
    private swagger: SwaggerClient,
    auth: EncryptDecryptService,
    confirmationService: ConfirmationService,
    toastr: MessageService) {
    super(router, auth, confirmationService, toastr);
  }

  returnDataFn(): Observable<ViewerPaginationOfLocalizationDetailsDTO> {
    return this.swagger.apiLocalizationGetLocalizationGet(
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
      id:0,
      key: '',
      valueAr: '',
      valueEn: '',
    } as LocalizationDetailsDTO
    this.display = true;
  }

  CancelDialog() {
    this.display = false;
  }

  Edit(entity: LocalizationDetailsDTO) {
    this.SelectEdentity = entity
    this.showDialog()
  }

  Delete(entity: LocalizationDetailsDTO) {
    this.swagger.apiLocalizationDeleteTranslationPost(entity.id).subscribe(res => {
      if (res) {
        this.getData()
      }
    })
  }


  Add() {
    if (this.SelectEdentity.key != '' && this.SelectEdentity.valueAr !== '') {
      this.swagger.apiLocalizationAddTranslationPost(this.SelectEdentity).subscribe(
        (response) => {
          this.getData()
          this.CancelDialog()
          this.SelectEdentity = {
            key: '',
            valueAr: '',
            valueEn: '',
          } as LocalizationDetailsDTO
        },
        (err: HttpErrorResponse) => {
        }
      );
    }
  }


}
