import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DatePipe, Location } from '@angular/common';
import { AbstractControl, FormBuilder, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { Country, EducationalLevel, PersonalInformationDto, Religion, ResidenceIssuePlace, SwaggerClient } from 'src/app/Shared/Services/Swagger/SwaggerClient.service';
import { EncryptDecryptService } from 'src/app/Shared/Services/encrypt-decrypt.service';
import { DefaultFormComponent } from 'src/app/Shared/helpers/default-form.component';
import { Observable, forkJoin } from 'rxjs';

@Component({
  selector: 'app-personal-information',
  templateUrl: './personal-information.component.html',
  styleUrls: ['./personal-information.component.scss']
})
export class PersonalInformationComponent extends DefaultFormComponent<PersonalInformationDto> {
  url: any

  date: any
  users: any[]
  selectedCity: any
  display: boolean = false;
  Countries: any[] = []
  Religions: Religion[] = []
  ResidencePlaceList: ResidenceIssuePlace[] = []
  EducationalClassify: EducationalLevel[] = []
  constructor(
    route: ActivatedRoute,
    fb: FormBuilder,
    router: Router,
    location: Location,
    toast: MessageService,
    private swagger: SwaggerClient,
    private Encryption: EncryptDecryptService,
    private datePipe: DatePipe) {
    super(route, fb, router, location, toast);

    this.url = this.route.snapshot.paramMap.get('id')
      ? this.Encryption.decryptUsingAES256(this.route.snapshot.paramMap.get('id')?.replace(/__/g, "/")) : 0


  }

  override ngOnInit(): void {
    forkJoin(this.swagger.apiLookupGetCountriesGet(),
      this.swagger.apiLookupGetEducationalLevelsGet(),
      this.swagger.apiLookupGetReligionsGet(),
      this.swagger.apiLookupGetResidencePalceGet()
    ).subscribe(res => {
      this.Countries = res[0];
      this.EducationalClassify = res[1];
      this.Religions = res[2];
      this.ResidencePlaceList = res[3];
    })
    this.initForm();

    this.checkAddEdit();
    this.getDataFromServer();
  }

  Gender: any[] = [{ 'key': 'ذكر', 'value': 1 }, { 'key': 'أنثى', 'value': 2 }];
  MaritalStatus: any[] = [
    { 'key': 'أعزب', 'value': 1 },
    { 'key': 'متزوج', 'value': 2 },
    { 'key': 'مطلق', 'value': 3 },
    { 'key': 'أرمل', 'value': 4 }
  ];


  override initForm(): void {
    this.form = this.fb.group({
      id: 0,
      muslimeId: [this.url, [Validators.nullValidator]],
      dateOfBirth: [, [Validators.required]],
      dateOfEntryKingdom: [null, [Validators.required]],
      placeOfBirth: [null, [Validators.required]],
      nationality: [null, [Validators.required]],
      gender: [null, [Validators.required]],
      previousReligion: [null, [Validators.required]],
      positionInFamily: [null, [Validators.nullValidator]],
      maritalStatus: [null, [Validators.nullValidator]],
      husbandName: [null, [Validators.nullValidator]],
      educationalLevel: [null, [Validators.required]],
      residenceNumber: [null, [Validators.required]],
      residenceIssueDate: [null, [Validators.required]],
      residenceIssuePlace: [null, [Validators.required]],
      passportNumber: [null, [Validators.required]],
      dateOfPassportIssue: [null, [Validators.required]],
      // placeOfPassportIssue: [null, [Validators.required]],
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
      this.router.navigate(["/muslime/contact-address", this.Encryption.encryptUsingAES256(response)])


    }
  }
  override postSubscribtion(): void { }
  override returnGetModelByIdFn(): Observable<any> {
    return this.swagger.apiMuslimeGetPersonalInformationGet(this.url)
  }
  override returnEditFn(): Observable<any> {
    
    this.form.get('muslimeId')?.patchValue(this.url);
    // this.form.get('dateOfBirth')?.patchValue(this.datePipe.transform(new Date(this.form.get('dateOfBirth')?.value), 'dd-MM-yy'));
    // this.form.get('dateOfEntryKingdom')?.patchValue(this.datePipe.transform(new Date(this.form.get('dateOfEntryKingdom')?.value), 'dd-MM-yy'));
    // this.form.get('residenceIssueDate')?.patchValue(this.datePipe.transform(new Date(this.form.get('residenceIssueDate')?.value), 'dd-MM-yy'));
    // this.form.get('dateOfPassportIssue')?.patchValue(this.datePipe.transform(new Date(this.form.get('dateOfPassportIssue')?.value), 'dd-MM-yy'));

    return this.swagger.apiMuslimeCreatePersonalInformationPost(this.form.value);
  }
  showDialog() {
    this.display = true;
  }

  CancelDialog() {
    this.display = false;
  }

  returnAddFn(): Observable<any> {
    // this.form.get('dateOfBirth')?.patchValue(this.datePipe.transform(new Date(this.form.get('dateOfBirth')?.value), 'dd-MM-yy'));
    // this.form.get('dateOfEntryKingdom')?.patchValue(this.datePipe.transform(new Date(this.form.get('dateOfEntryKingdom')?.value), 'dd-MM-yy'));
    // this.form.get('residenceIssueDate')?.patchValue(this.datePipe.transform(new Date(this.form.get('residenceIssueDate')?.value), 'dd-MM-yy'));
    // this.form.get('dateOfPassportIssue')?.patchValue(this.datePipe.transform(new Date(this.form.get('dateOfPassportIssue')?.value), 'dd-MM-yy'));

    return this.swagger.apiMuslimeCreatePersonalInformationPost(this.form.value);
  }

  BackCustom() {
    this.router.navigate(["/muslime/personal-data", this.route.snapshot.paramMap.get('id')])
  }


  eitherOrValidator(controlName1: string, controlName2: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const control1 = control.get(controlName1);
      const control2 = control.get(controlName2);
      if ((control1?.value && control1.value.trim() !== '') || (control2?.value && control2.value.trim() !== '')) {
        return null;
      }
      return { eitherOrRequired: true };
    };
  }



}
