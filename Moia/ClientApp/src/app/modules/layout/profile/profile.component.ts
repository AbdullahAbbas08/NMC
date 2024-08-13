import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { SwaggerClient, UserType } from 'src/app/Shared/Services/Swagger/SwaggerClient.service';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { CustomApiService } from 'src/app/Shared/Services/custom-api.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  User: any
  Permission: any
  UsersList: any[] = []
  form!: FormGroup;
  ChangePasswordform!: FormGroup;
  uploadedFiles: any[] = [];

  constructor(private authService: AuthService,
    private swagger: SwaggerClient,
    private fb: FormBuilder,
    private toast: MessageService,
    private CustomeApi: CustomApiService) {
    this.initForm()
    this.initChangePasswordform()
  }

  ngOnInit(): void {
    this.User = this.authService.User$.getValue();

    this.Permission = this.authService.getUserPermissions();
    console.log(this.Permission);
    
    this.getAllUsers()
    this.getNegotiatedUser()
  }

  initForm(): void {
    this.form = this.fb.group({
      title: [, [Validators.nullValidator]],
    });
  }

  initChangePasswordform(): void {
    this.ChangePasswordform = this.fb.group({
      oldPassword: [, [Validators.required]],
      newPassword: [, [Validators.required]],
    });
  }

  getAllUsers() {
    this.swagger.apiLookupGetAllNegotiatedUsersGet().subscribe(
      res => {
        this.UsersList = res;
      }
    )
  }

  NegoiateUser() {
    this.swagger.apiUserSaveNegotiateUserPost(this.form.get('title')?.value).subscribe(
      (response) => {
        if (!response) {
          this.toast.add({
            severity: 'error',
            detail: 'لم تتم العملية بنجاح',
          });
        } else {
          this.toast.add({
            severity: 'success',
            detail: 'تمت العملية بنجاح',
          });
          this.getAllUsers();
          this.getNegotiatedUser()

        }
      },
      (err: HttpErrorResponse) => {

      }
    );
  }

  changePassword() {
    this.swagger.apiUserChangePasswordPost(this.ChangePasswordform.value).subscribe(
      res => {
        if (!res) {
          this.toast.add({
            severity: 'error',
            detail: 'لم تتم العملية بنجاح',
          });
        } else {
          this.toast.add({
            severity: 'success',
            detail: 'تمت العملية بنجاح',
          });
        }
      }
    )
  }

  getNegotiatedUser() {
    this.swagger.apiUserGetNegotiatedUserGet().subscribe(
      (response) => {
        // console.log(response);

        this.form.get('title')?.patchValue(response)
      },
      (err: HttpErrorResponse) => { }
    );
  }

  onUpload(event: any) {
    for (let file of event.files) {
      this.uploadedFiles.push(file);
    }
    this.toast.add({ severity: 'info', summary: 'File Uploaded', detail: '' });
  }

}
