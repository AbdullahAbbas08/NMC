import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Observable } from 'rxjs';
import { BranshDto, CommitteeDto, CommitteeList, CommitteeRoleViewModel, DepartmentDto, MainUserNameIDList, SwaggerClient, UserDto, ViewerPaginationOfCommitteeList, ViewerPaginationOfWitness, Witness, WitnessDto } from 'src/app/Shared/Services/Swagger/SwaggerClient.service';
import { CustomApiService } from 'src/app/Shared/Services/custom-api.service';
import { EncryptDecryptService } from 'src/app/Shared/Services/encrypt-decrypt.service';
import { DefaultListComponent } from 'src/app/Shared/helpers/default-list.component';

@Component({
  selector: 'app-committee-list',
  templateUrl: './committee-list.component.html',
  styleUrls: ['./committee-list.component.scss']
})
export class CommitteeListComponent extends DefaultListComponent<ViewerPaginationOfCommitteeList, CommitteeList> {

  display: boolean = false;
  form!: FormGroup;

  DataEntries: CommitteeRoleViewModel[]  = []
  DataEntriesdisplay: boolean = false;
  DepartmentList: DepartmentDto[]
  UsersList: CommitteeRoleViewModel[]
  ExternalUserdisplay: boolean = false;
  model: UserDto;

  constructor(router: Router,
    private swagger: SwaggerClient,
    auth: EncryptDecryptService,
    confirmationService: ConfirmationService,
    private CustomApiService: CustomApiService,
    toastr: MessageService, private fb: FormBuilder,) {

    super(router, auth, confirmationService, toastr);
    this.initForm()
    this.getBranches()
    // this.getAllUsers()
    this.getAllDepartment()
    this.model = new UserDto()
  }

  initForm(): void {
    this.form = this.fb.group({
      id: 0,
      title: [null, [Validators.required]],
      contactNumber: [null, [Validators.required]],
      branchId: [null, [Validators.nullValidator]],
      // committeeManager: [, [Validators.required]],
      // departmentDto: [, [Validators.nullValidator]],
      // committeeDataEntryUsers: [, [Validators.required]],
    });
  }



  showDialog() {
    this.display = true;
  }

  showDataEntriesdisplayDialog() {
    this.DataEntriesdisplay = true;
  }

  CancelDialog() {
    this.display = false;
  }

  CancelDataEntriesdisplayDialog() {
    this.DataEntriesdisplay = false;
  }

  getAllDepartment() {
    this.swagger.apiLookupGetAllDepartmentsGet().subscribe(
      res => { this.DepartmentList = res }
    )
  }

  // getAllUsers() {
  //   this.swagger.apiLookupGetAllUsersGet().subscribe(
  //     res => {
  //       this.UsersList = res;
  //     }
  //   )
  // }

  // AddExternalUser() {
  //   this.model.activeDirectoryUser = false;
  //   this.model.userName = this.model.identity

  //   const formData = new FormData()
  //   formData.append("ID", (this.model.id ? this.model.id : 0) as unknown as Blob)
  //   formData.append("PasswordHash", (this.model.passwordHash ? this.model.passwordHash : null) as unknown as Blob)
  //   formData.append("UserName", this.model?.userName as unknown as Blob)
  //   formData.append("Mobile", (this.model.mobile ? this.model.mobile : null) as unknown as Blob)
  //   formData.append("Name", this.model?.name as unknown as Blob)
  //   formData.append("Identity", this.model?.identity as unknown as Blob)
  //   formData.append("Email", (this.model.email ? this.model.email : null) as unknown as Blob)
  //   formData.append("ActiveDirectoryUser", this.model?.activeDirectoryUser as unknown as Blob)
  //   formData.append("Branch", this.model?.branch as unknown as Blob)
  //   formData.append("BranchId", this.model?.branch?.id as unknown as Blob)
  //   formData.append("UserType", this.model.userType as unknown as Blob)
  //   formData.append("Signature", this.model.signature as unknown as Blob)

  //   this.CustomApiService.SaveUser(formData).subscribe(
  //     (response) => {
  //       this.getAllUsers()
  //       this.ExternalUserdisplay = false
  //     },
  //     (err: HttpErrorResponse) => {

  //     }
  //   );
  // }

  returnDataFn(): Observable<ViewerPaginationOfCommitteeList> {

    return this.swagger.apiCommitteeGetAllCustomGet(
      this.searchTermControl.value,
      this.page,
      this.pageSize
    );
  }
  returnDeleteFn(id: number): Observable<any> {
    return this.swagger.apiCommitteeDeletePost(id);
  }

  returnAddFn(): Observable<any> {
    return this.swagger.apiCommitteeInsertPost(this.form.value);
  }

  async GetCommiteeDataEntries(entity: CommitteeList) {
    this.DataEntries = entity.committeeDataEntryUsers ? entity.committeeDataEntryUsers:[]
    this.showDataEntriesdisplayDialog();
    // try {
    //   const res = await this.swagger.apiCommitteeGetCommiteeDataEntriesGet(id).toPromise();
    //   if (res)
    //     this.DataEntries = res
    //   this.showDataEntriesdisplayDialog();
    // } catch (error) {
    // }
  }


  addnew() {
    this.initForm()
    this.showDialog()
  }

  Add() {
    // console.log(this.form.value);

    this.swagger.apiCommitteeInsertPost(this.form.value).subscribe(
      (response) => {
        this.getData();
        this.CancelDialog()
      },
      (err: HttpErrorResponse) => {
      }
    );
  }

  async Edit(entity: CommitteeList) {
    // this.getAllUsers()
    this.form.patchValue(entity)
    this.showDialog();
  }
  BranchList: BranshDto[] = []

  getBranches() {
    this.swagger.apiLookupGetAllBranchsGet().subscribe(
      res => {
        this.BranchList = res
      }
    )
  }


}
