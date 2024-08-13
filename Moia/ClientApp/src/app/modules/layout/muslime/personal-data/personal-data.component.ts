import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PersonalDataDto, Preacher, SwaggerClient, Witness, WitnessDto } from '../../../../Shared/Services/Swagger/SwaggerClient.service';
import { Observable } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { DefaultFormComponent } from 'src/app/Shared/helpers/default-form.component';
import { DatePipe, Location } from '@angular/common';
import { AbstractControl, FormBuilder, FormControl, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { EncryptDecryptService } from 'src/app/Shared/Services/encrypt-decrypt.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-personal-data',
  templateUrl: './personal-data.component.html',
  styleUrls: ['./personal-data.component.scss']
})
export class PersonalDataComponent extends DefaultFormComponent<PersonalDataDto> {
  url: any = this.route.snapshot.paramMap.get('id')
    ? this.Encryption.decryptUsingAES256(this.route.snapshot.paramMap.get('id')?.replace(/__/g, "/")) : 0

  date: any
  users: any[]
  selectedCity: any
  display: boolean = false;
  Emamdisplay: boolean = false;
  model: Witness = new Witness();

  witnessList: WitnessDto[]
  PresherList: Preacher[]
  selectedWitness1: WitnessDto
  selectedWitness2: WitnessDto

  SelectEdentity: Preacher = {
    title: ''
  } as Preacher

  type: number

  Filter = {
    firstDropdown: null,
    secondDropdown: null
  };

  constructor(
    route: ActivatedRoute,
    fb: FormBuilder,
    router: Router,
    location: Location,
    toast: MessageService,
    private swagger: SwaggerClient,
    private Encryption: EncryptDecryptService,
    private datePipe: DatePipe,private translate: TranslateService) {
    super(route, fb, router, location, toast);
    this.getAllWitness();
    this.getAllPreshers();
  }

  // override ngOnInit(): void {
  //   this.initForm();
  //   this.checkAddEdit();
  //   this.getDataFromServer();
  // }

  override initForm(): void {

    this.form = this.fb.group({
      muslimeId: [this.url, [Validators.required]],
      nameBeforeFristAr: [null, [Validators.required]],
      nameBeforeMiddleAr: [null, [Validators.nullValidator]],
      nameBeforeLastAr: [null, [Validators.required]],
      nameBeforeFristEn: [null, [Validators.required]],
      nameBeforeMiddleEn: [null, [Validators.nullValidator]],
      nameBeforeLastEn: [null, [Validators.required]],
      nameAfter: [null, [Validators.required]],
      nameAfterEn: [null, [Validators.required]],
      islamDate: [null, [Validators.required]],
      // islamDateHijry: [null, [Validators.required]],
      firstWitness: [null, [Validators.required]],
      secondWitness: [null, [Validators.required]],
      preacherName: [null, [Validators.nullValidator]],
    });
  }
  override onAdd(): void { }
  override onEdit(): void { }
  onSave(response: boolean): void {
    
    if (!response) {
      this.toast.add({
        severity: 'error',
        detail: 'لم تتم العملية بنجاح',
      });
    }
    else {
      this.router.navigate(["/muslime/personal-information", this.Encryption.encryptUsingAES256(response)])

    }
  }
  override postSubscribtion(): void { }
  override returnGetModelByIdFn(): Observable<PersonalDataDto> { return this.swagger.apiMuslimeGetPersonalDataGet(this.url) }
  override returnEditFn(): Observable<any> {
    
    // return new Observable(observer => {
    //   const formData = this.form.value;
    //   this.router.navigateByUrl("/muslime/personal-information")
    //   observer.next(formData);
    //   observer.complete();
    // });
    // alert(this.form.get('islamDate')?.value)
    this.form.get('muslimeId')?.patchValue(this.url);

    // this.form.get('islamDate')?.patchValue(this.datePipe.transform(new Date(this.form.get('islamDate')?.value), 'dd-MM-yy'));
    // this.form.get('islamDateHijry')?.patchValue(this.datePipe.transform(new Date(this.form.get('islamDateHijry')?.value), 'dd-MM-yy'));

    return this.swagger.apiMuslimeCreatePersonalDataPost(this.form.value);
  }


  getAllWitness() {
    this.swagger.apiWitnessGetAllGet().subscribe(
      res => {
        this.witnessList = res;
      }
    )
  }

  getAllPreshers() {
    this.swagger.apiLookupGetPreshersGet().subscribe(
      res => {
        this.PresherList = res;
      }
    )
  }

  navigate() {
    this.router.navigateByUrl("/muslime/personal-information")
  }

  showDialog() {
    this.display = true;
  }

  CancelDialog() {
    this.display = false;
  }

  returnAddFn(): Observable<any> {
    // this.form.get('islamDate')?.patchValue(this.datePipe.transform(new Date(this.form.get('islamDate')?.value), 'dd-MM-yy'));
    // this.form.get('islamDateHijry')?.patchValue(this.datePipe.transform(new Date(this.form.get('islamDateHijry')?.value), 'dd-MM-yy'));

    return this.swagger.apiMuslimeCreatePersonalDataPost(this.form.value);
  }

  Add() {
    this.swagger.apiWitnessInsertPost(this.model).subscribe(
      (response) => {

        if (response.message != null && response.data == null) {
          this.toast.add({
            severity: 'error',
            detail: response.message,
          });
        } else {

          this.CancelDialog()
          this.model = new Witness()
          // this.getAllWitness()
          this.witnessList.push(response.data as WitnessDto);
          if (this.type == 1) this.form.get('firstWitness')?.patchValue(response.data as WitnessDto)
          else if (this.type == 2) this.form.get('secondWitness')?.patchValue(response.data as WitnessDto)

        }
      },
      (err: HttpErrorResponse) => {

      }
    );
  }

  AddPresher() {
    if (this.SelectEdentity.title != '') {
      this.swagger.apiLookupSavePresherPost(this.SelectEdentity).subscribe(
        (response) => {
          this.getAllPreshers()
          this.Emamdisplay = false
          this.SelectEdentity = {
            title: ''
          } as Preacher
        },
        (err: HttpErrorResponse) => {
        }
      );
    }
  }

 

  getFirstW(e: any) {
    this.Filter.firstDropdown = e.value.id;
   

    this.checkForDuplicate( this.form.controls['firstWitness']);
  }

  getSecondW(e: any) {
    this.Filter.secondDropdown = e.value.id;
    this.checkForDuplicate(this.form.controls['secondWitness']);
  }

  checkForDuplicate(control:AbstractControl) {
    if (this.Filter.firstDropdown && this.Filter.secondDropdown) {
      if (this.Filter.firstDropdown === this.Filter.secondDropdown) {
        control.reset()
        this.toast.add({
          severity: 'error',
          detail: this.translate.instant("sorryYouCannotChooseTheSameWitness"),
        });  

          }
    }
  }


}
