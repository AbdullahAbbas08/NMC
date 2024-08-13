import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Observable } from 'rxjs';
import { BranshListDto, CommitteeRoleViewModel, DepartmentDto, NameIdViewModel, SwaggerClient, ViewerPaginationOfBranshListDto, ViewerPaginationOfDepartmentDto } from 'src/app/Shared/Services/Swagger/SwaggerClient.service';
import { EncryptDecryptService } from 'src/app/Shared/Services/encrypt-decrypt.service';
import { DefaultListComponent } from 'src/app/Shared/helpers/default-list.component';

@Component({
  selector: 'app-branch-list',
  templateUrl: './branch-list.component.html',
  styleUrls: ['./branch-list.component.scss']
})
export class BranchListComponent extends DefaultListComponent<ViewerPaginationOfBranshListDto, BranshListDto> {

  display: boolean = false;
  model: any
  manager: NameIdViewModel
  SelectEdentity: BranshListDto = new BranshListDto()
  UsersList: CommitteeRoleViewModel[]

  constructor(router: Router,
    private swagger: SwaggerClient,
    auth: EncryptDecryptService,
    confirmationService: ConfirmationService,
    toastr: MessageService) {
    super(router, auth, confirmationService, toastr);
    this.getAllUsers()
  }

  returnDataFn(): Observable<ViewerPaginationOfBranshListDto> {

    return this.swagger.apiLookupGetAllBranchsPaginatedGet(
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

  Edit(entity: BranshListDto) {
    this.SelectEdentity = entity
    this.model = entity.manager
    this.showDialog()
  }


  Add() {
    if (this.manager) {
      this.swagger.apiUserUpdateBrMPost(this.manager.id, this.SelectEdentity.id).subscribe(
        (response) => {
          this.getData()
          this.CancelDialog()
          this.SelectEdentity = new BranshListDto()
          this.manager = new NameIdViewModel()
          this.getAllUsers()
        },
        (err: HttpErrorResponse) => {
        }
      );
    }
  }


}
