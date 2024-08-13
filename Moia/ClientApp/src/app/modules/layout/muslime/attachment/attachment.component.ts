import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DatePipe, Location } from '@angular/common';
import { DefaultFormComponent } from 'src/app/Shared/helpers/default-form.component';
import { FileParameter, ImageType, OrderStage, OrderStatus, PersonalDataDto, SwaggerClient } from 'src/app/Shared/Services/Swagger/SwaggerClient.service';
import { FormBuilder, Validators } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { EncryptDecryptService } from 'src/app/Shared/Services/encrypt-decrypt.service';
import { Observable } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { CustomApiService } from 'src/app/Shared/Services/custom-api.service';
import { environment } from 'src/environments/environment.prod';
import { AuthService } from 'src/app/Shared/Services/auth.service';
import { TranslateService } from '@ngx-translate/core';
import { log } from 'console';

@Component({
  selector: 'app-attachment',
  templateUrl: './attachment.component.html',
  styleUrls: ['./attachment.component.scss']
})
export class AttachmentComponent extends DefaultFormComponent<PersonalDataDto> {
  url: any = this.route.snapshot.paramMap.get('id')
    ? this.Encryption.decryptUsingAES256(this.route.snapshot.paramMap.get('id')?.replace(/__/g, "/")) : 0
    orderId:any
  formData: FormData = new FormData();
  personal: string = ''
  accomodation: string = ''
  passport: string = ''
  // OrderSend: boolean = true
  selectedImage:any
  selectedName:any
  displaySelectedImage: boolean = false;
  responseCheck:boolean = true
  constructor(
    route: ActivatedRoute,
    fb: FormBuilder,
    router: Router,
    location: Location,
    toast: MessageService,
    private swagger: SwaggerClient,
    private Encryption: EncryptDecryptService,
    private ApiService: CustomApiService,
    private authService: AuthService,
    private confirmationService: ConfirmationService,
    private translate: TranslateService) {
    super(route, fb, router, location, toast);
  }

  override ngOnInit(): void {
    this.initForm();
    this.checkAddEdit();
    this.getDataFromServer();
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      this.orderId=id   
    })
    
  }

  override initForm(): void {

    this.form = this.fb.group({
      muslimeId: [this.route.snapshot.paramMap.get('id') ? this.url : 0, [Validators.required]],
      personal: [null, [Validators.required]],
      accomodation: [null, [Validators.required]],
      passport: [null, [Validators.required]],

    });
  }
  override onAdd(): void { }
  override onEdit(): void { }
  onSave(response: any): void {
    
    this.formData = new FormData()
    if (response == null) {
      this.toast.add({
        severity: 'error',
        detail: 'لم تتم العملية بنجاح',
      });
    }
    else {
      this.save(response)
     
  }
}
  override postSubscribtion(): void {
  }
isDisabledBtn(res:any){
    if(this.url){
        if(!res?._Personal || !res?._Passport || !res?._Accomodation){
          this.responseCheck = true;
          
        } else {
          this.responseCheck = false;
        }
    }
  }
  override returnGetModelByIdFn(): Observable<any> {
    this.swagger.apiMuslimeGetattachmentDtoGet(this.url).subscribe(res => {

      if (res && res?._Personal) this.selectedImagePersonal = "data:image/png;base64," + res?._Personal
      if (res && res?._Passport) this.selectedImagePassport = "data:image/png;base64," + res?._Passport
      if (res && res?._Accomodation) this.selectedImageAccommodation = "data:image/png;base64," + res?._Accomodation

      this.personal = this.translate.instant('personalfoto')
      this.accomodation = this.translate.instant('accomodationfoto')
      this.passport = this.translate.instant('passportfoto')
      this.isDisabledBtn(res)

    })
    return this.swagger.apiMuslimeGetattachmentDtoGet(this.url)
  }

  override returnEditFn(): Observable<any> {
    
    this.formData.append("MuslimeId", this.url)
    return this.ApiService.apiMuslimeInsertAttachment(this.formData);
  }

  returnAddFn(): Observable<any> {
    //Save Order
    this.formData.append("MuslimeId", this.url)
    return this.ApiService.apiMuslimeInsertAttachment(this.formData);
  }


  selectedImagePersonal: string | null;
  selectedImageAccommodation: string | null;
  selectedImagePassport: string | null;

  addOrUpdateFormData(key: string, file: File) {
    // Check if key exists
    if (this.formData.has(key)) {
      // Remove the existing key
      this.formData.delete(key);
    }
    // Add the new key-value pair
    this.formData.append(key, file);
  }

  uploadFile(event: any, _ImageType: ImageType) {

    const file = event.target.files[0];
    var reader = new FileReader();
    reader.readAsDataURL(file);

    // Check file type
    const allowedTypes = ['image/jpeg', 'image/png'];
    if (!allowedTypes.includes(file.type)) {
      alert('Only JPEG and PNG files are allowed.');

      this.form.get('personal')?.patchValue(null)
      this.form.get('accomodation')?.patchValue(null)
      this.form.get('passport')?.patchValue(null)

      return;
    }

    // Check file size (3MB max)
    const maxSize = 3 * 1024 * 1024; // 3 MB in bytes
    if (file.size > maxSize) {
      alert('File size exceeds the limit of 3 MB.');
      this.form.get('personal')?.patchValue(null)
      this.form.get('accomodation')?.patchValue(null)
      this.form.get('passport')?.patchValue(null)
      return;
    }


    reader.onload = (_event) => {

      if (_ImageType == ImageType.Personal) {
        this.personal = this.translate.instant('personalfoto')
        this.selectedImagePersonal = reader.result as string;
        this.addOrUpdateFormData('Personal', file);
        // this.formData.append("Personal", file)
      }
      else if (_ImageType == ImageType.Accommodation) {
        this.accomodation = this.translate.instant('accomodationfoto')
        this.selectedImageAccommodation = reader.result as string;
        this.addOrUpdateFormData("Accomodation", file)
        // this.formData.append("Accomodation", file)
      }
      else if (_ImageType == ImageType.Passport) {
        this.passport = this.translate.instant('passportfoto')
        this.selectedImagePassport = reader.result as string;
        this.addOrUpdateFormData("Passport", file)
        // this.formData.append("Passport", file)
      }
    };
  }


  override submit() {
    this.isLoadingBtn = true;
    this.returnEditFn().subscribe(
      (response) => {
        response &&
          this.toast.add({
            severity: 'success',
            detail: 'تمت العملية بنجاح',
          });
        this.onSave(response);
        this.isLoadingBtn = false;
      },
      (err: HttpErrorResponse) => {
        this.isLoadingBtn = false;
      }
    );
  }


  confirm() {
    this.confirmationService.confirm({
      message: 'هل أنت متأكد من إرسال الطلب',
      key: 'إلغاء',
      accept: () => {
        this.isLoadingBtn = true;
        this.returnEditFn().subscribe(
          (response) => {
            response &&
              this.toast.add({
                severity: 'success',
                detail: 'تمت العملية بنجاح',
              });
            this.onSave(response);
            this.isLoadingBtn = false;
          },
          (err: HttpErrorResponse) => {
            this.isLoadingBtn = false;
          }
        )
      },
    });
  }

  save(res:any) {
    this.formData.append("MuslimeId", this.url)
    this.ApiService.apiMuslimeInsertAttachment(this.formData).subscribe(res => {
      
      if (res == null) {
        this.toast.add({
          severity: 'error',
          detail: 'لم تتم العملية بنجاح',
        });
      } else {

        this.router.navigate(["/muslime/confirm-request",this.Encryption.encryptUsingAES256(res.code)])


      }
    })
  }


  previewImage(path:any,selectedName:any,e:Event){
    
    this.selectedImage =path
    this.selectedName = selectedName
    this.displaySelectedImage = true
    e.stopPropagation()
    
    
  }

  navigate(){
    this.router.navigateByUrl("/muslime/confirm-request")


  }
}
