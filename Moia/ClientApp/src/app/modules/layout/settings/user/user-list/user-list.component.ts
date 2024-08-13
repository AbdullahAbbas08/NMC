import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { debounceTime, distinctUntilChanged, Observable, switchMap, tap } from 'rxjs';
import { BranshDto, CommitteeIdName, DepartmentDto, FilterData, MainRoleNameId, MainUserDto, SwaggerClient, UserDto, UserType, ViewerPaginationOfMainUserDto, ViewerPaginationOfWitness, Witness } from 'src/app/Shared/Services/Swagger/SwaggerClient.service';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CustomApiService } from 'src/app/Shared/Services/custom-api.service';
import { EncryptDecryptService } from 'src/app/Shared/Services/encrypt-decrypt.service';
import { DefaultListComponent } from 'src/app/Shared/helpers/default-list.component';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent extends DefaultListComponent<ViewerPaginationOfMainUserDto, MainUserDto> {

  display: boolean = false;
  ExternalUserdisplay: boolean = false;
  internalUserdisplay: boolean = false;
  model: UserDto;
  logo: string | ArrayBuffer | null = null;
  userRole: any = { type: "أخر", value: 9 }
  activeDirectoryUser: Boolean = false
  BranchList: BranshDto[] = []
  selectedBranch: BranshDto
  selectedRole: MainRoleNameId
  passwordDisable: boolean = false
  usernameDisable: boolean = true
  Active: boolean | null = true
  StatusList: any[] = [
    { type: this.translate.instant('All'), value: 3 },
    { type: this.translate.instant('Allactive'), value: 1 },
    { type: this.translate.instant('Allblock'), value: 2 },
  ]

  CommitteeList: CommitteeIdName[] = []
  BranchCommitteeList: CommitteeIdName[] = []
  SelectedCommittee: CommitteeIdName
  User: any

  departmentList: DepartmentDto[] = []
  Selecteddepartment: DepartmentDto

  Filter:FilterData = {
    page:this.page,
    pageSize:this.pageSize,
    searchTerm:this.searchTermControl.value,
    active:3,
  } as FilterData;
  
  constructor(router: Router,
    private swagger: SwaggerClient,
    auth: EncryptDecryptService,
    private authService: AuthService,
    confirmationService: ConfirmationService,
    private translate: TranslateService,
    private CustomApiService: CustomApiService,
    toastr: MessageService) {
    super(router, auth, confirmationService, toastr);
    this.User = this.authService.User$.getValue();
    // console.log(this.User);
    if (this.User?.Role?.Name == 'BranchDataEntry')
      this.getCommitteesByBranchId(this.User?.Branch?.ID)


    this.model = new UserDto()
    this.getBranches()
    this.getCommittees()
    this.getDepartment()
    this.GetMainRoles()
    this.GetAllMainRoles()
    this.GetCommitteeMainRoles()

  }


  getCommittees() {
    this.swagger.apiCommitteeGetAllCommitteesGet().subscribe(
      res => {
        this.CommitteeList = res
      }
    )
  }

  getCommitteesByBranchId(brId: any) {
    this.swagger.apiCommitteeGetAllCommitteesByBranchIdGet(brId).subscribe(
      res => {
        this.BranchCommitteeList = res
      }
    )
  }

  getCommitteesByBranch() {
    this.getCommitteesByBranchId(this.model.branch?.id)
  }

  onSelect(event: any) {
    const selectedFile = event.target.files[0];
    if (selectedFile) {
      const reader = new FileReader();
      reader.onload = () => {
        const fileContentAsBase64 = reader.result as string;
        this.model.signature = selectedFile
        this.logo = fileContentAsBase64;
      };
      reader.readAsDataURL(selectedFile);
    }
  }

  getDepartment() {
    this.swagger.apiLookupGetAllDepartmentsGet().subscribe(
      res => {
        this.departmentList = res
      }
    )
  }


  filterFn() {
    this.getData()
  }


  showDialog() {
    this.logo = null
    this.display = true;
  }

  CancelDialog() {
    this.display = false;
  }

  returnDataFn(): Observable<ViewerPaginationOfMainUserDto> {
    this.Filter.searchTerm = this.searchTermControl.value;
    this.Filter.page = this.page ;
    this.Filter.pageSize = this.pageSize ;
    
    if( this.Filter.searchTerm)
      this.Filter.active = 3
    
    return this.swagger.apiUserGetAllCustomPost(this.Filter );
  }


  returnDeleteFn(id: number): Observable<any> {
    return this.swagger.apiWitnessDeletePost(id);
  }

  returnAddFn(): Observable<any> {
    const formData = new FormData()
    formData.append("ID", (this.model.id ? this.model.id : 0) as unknown as Blob)
    formData.append("PasswordHash", (this.model.passwordHash ? this.model.passwordHash : null) as unknown as Blob)
    formData.append("UserName", this.model?.userName as unknown as Blob)
    formData.append("Mobile", (this.model.mobile ? this.model.mobile : null) as unknown as Blob)
    formData.append("Name", this.model?.name as unknown as Blob)
    formData.append("Identity", this.model?.identity as unknown as Blob)
    formData.append("Email", (this.model.email ? this.model.email : null) as unknown as Blob)
    formData.append("ActiveDirectoryUser", this.model?.activeDirectoryUser as unknown as Blob)
    formData.append("Branch", this.model?.branch as unknown as Blob)
    if (this.model?.branch?.id)
      formData.append("BranchId", (this.model?.branch?.id) as unknown as Blob)
    formData.append("UserType", this.model.userType as unknown as Blob)
    formData.append("RoleId", (this.model.role ? this.model.role.id : 0) as unknown as Blob)
    formData.append("Signature", this.model.signature as unknown as Blob)

    return this.CustomApiService.SaveUser(formData);
  }


  async GetModelById(id: any) {
    try {
      const res = await this.swagger.apiUserGetByIdCustomeGet(id).toPromise();
      this.model = res?.toJSON()
      this.getCommitteesByBranchId(this.model.branchId)
      // console.log(this.model);
      this.logo = this.model.attachmentBase64 as string;

      if (this.model.activeDirectoryUser) this.internalUserdisplay = true;
      else this.ExternalUserdisplay = true
    } catch (error) {
    }
  }


  addnew() {
    this.model = new UserDto()
    this.showDialog()
  }

  reset(){

    this.StatusList = [
      { type: this.translate.instant('All'), value: 3 },
      { type: this.translate.instant('Allactive'), value: 1 },
      { type: this.translate.instant('Allblock'), value: 2 },
    ]
    
    this.searchTermControl.patchValue('')
    this. Filter = {
      page:this.page,
      pageSize:this.pageSize,
      searchTerm:'',
      active:3,
    } as FilterData;

  }

  AddExternalUser() {
    this.model.activeDirectoryUser = false;
    this.model.userName = this.model.identity
    if (this.model.name && this.model.userName && this.model.identity) {

      const formData = new FormData()
      formData.append("ID", (this.model.id ? this.model.id : 0) as unknown as Blob)
      formData.append("PasswordHash", (this.model.passwordHash ? this.model.passwordHash : null) as unknown as Blob)
      formData.append("UserName", this.model?.userName as unknown as Blob)
      formData.append("Mobile", (this.model.mobile ? this.model.mobile : null) as unknown as Blob)
      formData.append("Name", this.model?.name as unknown as Blob)
      formData.append("Identity", this.model?.identity as unknown as Blob)
      formData.append("Email", (this.model.email ? this.model.email : null) as unknown as Blob)
      formData.append("ActiveDirectoryUser", this.model?.activeDirectoryUser as unknown as Blob)
      formData.append("Branch", this.model?.branch as unknown as Blob)
      if (this.model?.branch?.id)
        formData.append("BranchId", (this.model?.branch?.id) as unknown as Blob)
      formData.append("UserType", 9 as unknown as Blob)
      formData.append("Signature", this.model.signature as unknown as Blob)
      formData.append("RoleId", (this.model.role ? this.model.role.id : 0) as unknown as Blob)
      formData.append("CommitteeId", (this.model.committeeId ? this.model.committeeId : 0) as unknown as Blob)


      this.CustomApiService.SaveUser(formData).subscribe(
        (response) => {
          if (!response) {
            this.toast.add({
              severity: 'error',
              detail: 'لم تتم العملية بنجاح برجاء مراجعة البيانات بشكل صحيح',
            });
          }
          else {
            this.getData();
            this.CancelDialog()
            this.ExternalUserdisplay = false
            this.internalUserdisplay = false
          }
        },
        (err: HttpErrorResponse) => {
        }
      );
    }
  }

  checkDataIsValid(): boolean {
    return !!this.model?.name && !!this.model?.userName && !!this.model?.identity;
  }

  setRole() {
    if(this.model.role?.name == 'CommitteeManager'){
      this.confirmationService.confirm({
        message: this.translate.instant('MoveOrdersBeforeChangeRoleConfirm'),
        key: 'MoveOrdersConfirm',
      });
    }
  }

  AddInternalUser() {

    this.model.activeDirectoryUser = true;
    if (this.userRole.value == 8)
      this.model.userType = UserType.BranchDataEntry
    else if (this.userRole.value == 3)
      this.model.userType = UserType.DepartmentManager
    else if (this.userRole.value == 4)
      this.model.userType = UserType.BranchManager
    else this.model.userType = UserType.None

    if (this.model.name &&
      this.model.userName &&
      this.userRole &&
      this.model.identity) {
      const formData = new FormData()
      formData.append("ID", (this.model.id ? this.model.id : 0) as unknown as Blob)
      formData.append("PasswordHash", (this.model.passwordHash ? this.model.passwordHash : null) as unknown as Blob)
      formData.append("UserName", this.model?.userName as unknown as Blob)
      formData.append("Mobile", (this.model.mobile ? this.model.mobile : null) as unknown as Blob)
      formData.append("Name", this.model?.name as unknown as Blob)
      formData.append("Identity", this.model?.identity as unknown as Blob)
      formData.append("Email", (this.model.email ? this.model.email : null) as unknown as Blob)
      formData.append("ActiveDirectoryUser", this.model?.activeDirectoryUser as unknown as Blob)
      formData.append("Branch", this.model?.branch as unknown as Blob)
      if (this.model?.branch?.id)
        formData.append("BranchId", (this.model?.branch?.id) as unknown as Blob)
      formData.append("UserType", this.model.userType as unknown as Blob)
      formData.append("Signature", this.model.signature as unknown as Blob)
      formData.append("RoleId", (this.model.role ? this.model.role.id : 0) as unknown as Blob)


      this.CustomApiService.SaveUser(formData).subscribe(
        (response) => {
          if (!response) {
            this.toast.add({
              severity: 'error',
              detail: response,
              // detail: 'لم تتم العملية بنجاح برجاء مراجعة البيانات بشكل صحيح',
            });
          }
          else {
            this.getData();
            this.CancelDialog()
            this.ExternalUserdisplay = false
            this.internalUserdisplay = false
          }
        },
        (err: HttpErrorResponse) => {
        }
      );
    }
  }

  async Edit(id: any) {
    await this.GetModelById(id)
    const formData = new FormData()
    formData.append("ID", (this.model.id ? this.model.id : 0) as unknown as Blob)
    formData.append("PasswordHash", (this.model.passwordHash ? this.model.passwordHash : null) as unknown as Blob)
    formData.append("UserName", this.model?.userName as unknown as Blob)
    formData.append("Mobile", (this.model.mobile ? this.model.mobile : null) as unknown as Blob)
    formData.append("Name", this.model?.name as unknown as Blob)
    formData.append("Identity", this.model?.identity as unknown as Blob)
    formData.append("Email", (this.model.email ? this.model.email : null) as unknown as Blob)
    formData.append("ActiveDirectoryUser", this.model?.activeDirectoryUser as unknown as Blob)
    formData.append("Branch", this.model?.branch as unknown as Blob)
    if (this.model?.branch?.id)
      formData.append("BranchId", (this.model?.branch?.id) as unknown as Blob)
    formData.append("UserType", this.model.userType as unknown as Blob)
    formData.append("Signature", this.model.signature as unknown as Blob)
    formData.append("RoleId", (this.model.role ? this.model.role.id : 0) as unknown as Blob)


    return this.CustomApiService.SaveUser(formData);
  }

  getBranches() {
    this.swagger.apiLookupGetAllBranchsGet().subscribe(
      res => {
        this.BranchList = res
      }
    )
  }

  AllRoles: MainRoleNameId[] = []
  roles: MainRoleNameId[] = []
  selected_role: MainRoleNameId
  GetMainRoles() {
    this.swagger.apiLookupGetMainRolesGet().subscribe(
      res => {
        this.roles = res
      }
    )
  }

  GetAllMainRoles() {
    this.swagger.apiLookupGetAllMainRolesGet().subscribe(
      res => {
        this.AllRoles = res
      }
    )
  }


  CommitteeRoles: MainRoleNameId[] = []
  selected_Committee_role: MainRoleNameId
  GetCommitteeMainRoles() {
    this.swagger.apiLookupGetCommitteeMainRolesGet().subscribe(
      res => {
        this.CommitteeRoles = res
      }
    )
  }

  ChangeState(userId: any, state: any) {
    this.swagger.apiUserActivateUserPost(userId, state).subscribe(
      res => {
        if (res)
          this.getData()
      }
    )
  }



}
