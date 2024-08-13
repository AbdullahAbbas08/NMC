import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Observable } from 'rxjs';
import { CommitteeRoleViewModel, DepartmentDto, NameIdViewModel, SwaggerClient, ViewerPaginationOfDepartmentDto } from 'src/app/Shared/Services/Swagger/SwaggerClient.service';
import { EncryptDecryptService } from 'src/app/Shared/Services/encrypt-decrypt.service';
import { DefaultListComponent } from 'src/app/Shared/helpers/default-list.component';

@Component({
  selector: 'app-department-list',
  templateUrl: './department-list.component.html',
  styleUrls: ['./department-list.component.scss']
})
export class DepartmentListComponent extends DefaultListComponent<ViewerPaginationOfDepartmentDto, DepartmentDto> {

  display: boolean = false;
  model: any
  manager: NameIdViewModel
  SelectEdentity: DepartmentDto = new DepartmentDto()
  UsersList: CommitteeRoleViewModel[]

  constructor(router: Router,
    private swagger: SwaggerClient,
    auth: EncryptDecryptService,
    confirmationService: ConfirmationService,
    toastr: MessageService) {
    super(router, auth, confirmationService, toastr);
    this.getAllUsers()
  }

  returnDataFn(): Observable<ViewerPaginationOfDepartmentDto> {

    return this.swagger.apiLookupGetAllDepartmentsPaginatedGet(
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

  getAllUsers() {
    this.swagger.apiLookupGetADUsersGet().subscribe(
      res => {
        this.UsersList = res;
      }
    )
  }

  Edit(entity: DepartmentDto) {
    this.SelectEdentity = entity
    this.model = entity.manager
    this.showDialog()
  }


  Add() {
    if (this.manager) {
      this.swagger.apiUserUpdateDeptMPost(this.manager.id, this.SelectEdentity.id).subscribe(
        (response) => {
          this.getData()
          this.CancelDialog()
          this.SelectEdentity = new DepartmentDto()
          this.manager = new NameIdViewModel()
        },
        (err: HttpErrorResponse) => {
        }
      );
    }
  }


}
