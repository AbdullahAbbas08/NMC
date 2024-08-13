import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { ChangePasswordModel, Settings, SwaggerClient, UserLoginModel } from 'src/app/Shared/Services/Swagger/SwaggerClient.service';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { EncryptDecryptService } from 'src/app/Shared/Services/encrypt-decrypt.service';
// import * as ressss from 'src/assets//appsettings.json';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  changePasswordForm: FormGroup;
  resetPasswordForm: FormGroup;
  OTPForm: FormGroup;
  ResetPassForm: FormGroup;
  isLoading: boolean = false;
  isLoadingChangePassword: boolean = false;
  isLoadingresetPassword: boolean = false;
  rememberMe: boolean = true;
  OTP_MODE: boolean = false;
  Login_MODE: boolean = true;
  ResetPass_MODE: boolean = false;
  AppSettings: any;
  ParsedRes: any
  settings: Settings[] = []

  programTypes: any[] = [
    { 'key': 1, 'value': this.translate.instant('HajjProgram') },
    { 'key': 2, 'value': this.translate.instant('OmurahProgram') }
  ];


  constructor(
    private fb: FormBuilder,
    private swagger: SwaggerClient,
    private authSvr: AuthService,
    public translate: TranslateService,
    private toast: MessageService,
    private EncryptDecrypt: EncryptDecryptService,
  ) {

    this.loginForm = this.createFormItem('init');
    this.OTPForm = this.createFormItem('OTPForm');
  }

  ngOnInit(): void {

  }


  createFormItem(itemType: string): FormGroup {
    let formItem = this.fb.group({});
    switch (itemType) {
      case 'init':
        formItem = this.fb.group({
          username: ['', Validators.required],
          password: ['', Validators.required],
        });
        break;
      case 'ChangePassword':
        formItem = this.fb.group({
          newPassword: ['', [Validators.required, Validators.minLength(6), Validators.pattern('(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&].{6,}')]],
          reNewPassword: ['', [Validators.required]],
        }, { validator: this.passwordMatchValidator });
        break;
      case 'resetPasswordForm':
        formItem = this.fb.group({
          newPassword: ['', [Validators.required,
            Validators.minLength(6),
            Validators.pattern('(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&].{6,}')]],
          reNewPassword: ['', [Validators.required]],
        }, { validator: this.passwordMatchValidator });
        break;
      case 'OTPForm':
        formItem = this.fb.group({
          otp: ['', Validators.required, [Validators.minLength(4), Validators.maxLength(4)]],
        });
        break;
    }
    return formItem;
  }

  passwordMatchValidator(formGroup: FormGroup) {
    const password = formGroup.get('newPassword')?.value;
    const confirmPassword = formGroup.get('reNewPassword')?.value;

    if (password !== confirmPassword) {
      formGroup.get('reNewPassword')?.setErrors({ passwordMismatch: true });
    } else {
      formGroup.get('reNewPassword')?.setErrors(null);
    }
  }



  onSubmit() {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
    } else {

      this.isLoading = true;
      this.swagger
        .apiUserLoginPost(
          // this.translate.currentLang,
          this.loginForm.value as UserLoginModel
        )
        .subscribe(
          (res: any) => {
            this.ParsedRes = JSON.parse(this.EncryptDecrypt.decryptUsingAES256(
              this.EncryptDecrypt.unshiftString(res, 6)
            ));

            if (this.ParsedRes.Token && this.ParsedRes.refreshToken) {
              this.swagger.apiSettingsGetSettingsGet().subscribe(
                (res: any) => {
                  this.settings = res;
                  this.OTP_MODE = this.settings.find(x => x.code == 'OTP')?.value == 'Enable'
                  if (this.OTP_MODE) {
                    this.Login_MODE = false
                    this.ResetPass_MODE = false
                    this.isLoading = false;
                  }
                  else {
                    if (!this.ParsedRes.Passchanged && !this.ParsedRes.IsActiveDirectoy) {
                      this.ResetPass_MODE = true
                      this.Login_MODE = false
                      this.OTP_MODE = false
                      this.isLoading = false;
                      this.ResetPassForm = this.createFormItem('resetPasswordForm');
                    }
                    else
                      this.authSvr.login(this.ParsedRes.Token, this.ParsedRes.refreshToken, this.rememberMe);
                  }
                }
              );


            }
            // && Number(this.EncryptDecrypt.decryptUsingAES256(ParsedRes.count)) == 0
            else if (this.ParsedRes.Token && this.ParsedRes.refreshToken) {
              this.toast.add({
                severity: 'error',
                detail: 'غير مسموح للدخول على النظام',
              });
              this.isLoading = false;
            }
            else {
              if (this.ParsedRes.Message === 'InvalidUsernameOrPassword') {
                this.toast.add({
                  severity: 'error',
                  detail: 'خطأ فى إدخال البيانات برجاء كتابة البيانات بشكل صحيح',
                });
              }
              else if (this.ParsedRes.Message === 'LockAccount') {
                this.toast.add({
                  severity: 'error',
                  detail: 'تم غلق الحساب برجاء مراجعة الدعم الفنى',
                });
              }
              else {
                this.toast.add({
                  severity: 'error',
                  detail: 'ليس لديك صلاحية الدخول على النظام',
                });
              }
              this.isLoading = false;
            }

          },
          (err: HttpErrorResponse) => {
            this.isLoading = false;
          }
        );
    }
  }

  CheckOTP() {
    this.swagger.apiUserCheckOTPPost(this.loginForm.get('username')?.value, this.OTPForm.get('otp')?.value).subscribe(res => {
      let _res = this.EncryptDecrypt.decryptUsingAES256(res)

      if (_res == 'True') {
        if (!this.ParsedRes.Passchanged && !this.ParsedRes.IsActiveDirectoy) {
          this.ResetPass_MODE = true
          this.Login_MODE = false
          this.OTP_MODE = false
          this.isLoading = false;
        }
        else
          this.authSvr.login(this.ParsedRes.Token, this.ParsedRes.refreshToken, this.rememberMe);
      } else if (_res == 'LockAccount') {
        this.toast.add({
          severity: 'error',
          detail: 'تم غلق الحساب برجاء مراجعة الدعم الفنى',
        });
      }
      else {
        this.toast.add({
          severity: 'error',
          detail: 'الكود غير صالح أعد المحاولة مرة أخرى',
        });
      }


    })
  }

  CheckPass() {
    this.swagger.apiUserChangePasswordPost(
      {
        oldPassword: this.loginForm.get('password')?.value,
        username: this.loginForm.get('username')?.value,
        newPassword: this.ResetPassForm.get('newPassword')?.value
      } as ChangePasswordModel
    ).subscribe(res => {
      if (res) {
        this.authSvr.login(this.ParsedRes.Token, this.ParsedRes.refreshToken, this.rememberMe);
      } else {
        this.toast.add({
          severity: 'error',
          detail: 'حدث خطأ أعد المحاولة',
        });
      }


    })
  }

  DisplayresetPasswordModal: boolean = false;
  showresetPasswordDialog() {
    this.DisplayresetPasswordModal = true;
  }

  DisplayforgetPasswordModal: boolean = false;
  showforgetPasswordDialog() {

    this.DisplayforgetPasswordModal = true;
  }

}
