import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DatePipe, Location } from '@angular/common';
import { DefaultFormComponent } from 'src/app/Shared/helpers/default-form.component';
import { FormBuilder, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { ContactAndInfoDataViewModel, Country, SwaggerClient } from 'src/app/Shared/Services/Swagger/SwaggerClient.service';
import { EncryptDecryptService } from 'src/app/Shared/Services/encrypt-decrypt.service';
import { Observable, forkJoin } from 'rxjs';

@Component({
  selector: 'app-contact-address-data',
  templateUrl: './contact-address-data.component.html',
  styleUrls: ['./contact-address-data.component.scss']
})
export class ContactAddressDataComponent extends DefaultFormComponent<ContactAndInfoDataViewModel> {
  url: any = this.route.snapshot.paramMap.get('id')
    ? this.Encryption.decryptUsingAES256(this.route.snapshot.paramMap.get('id')?.replace(/__/g, "/")) : 0

  date: any
  users: any[]
  selectedCity: any
  display: boolean = false;
  Countries: Country[]

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
    forkJoin(this.swagger.apiLookupGetCountriesGet(),
      this.swagger.apiLookupGetEducationalLevelsGet(),
      this.swagger.apiLookupGetReligionsGet()).subscribe(res => {
        this.Countries = res[0];
      })
  }

  override ngOnInit(): void {
    this.initForm();
    this.checkAddEdit();
    this.getDataFromServer();
  }

  override initForm(): void {

    this.form = this.fb.group({
      muslimeId: [this.url, [Validators.nullValidator]],
      originalCountry: this.fb.group({
        country: [null, [Validators.nullValidator]],
        city: [null, [Validators.nullValidator]],
        street: [null, [Validators.nullValidator]],
        region: [null, [Validators.nullValidator]],
        doorNumber: [null, [Validators.nullValidator]],
      }),
      currentResidence: this.fb.group({
        city: [null, [Validators.required]],
        street: [null, [Validators.nullValidator]],
        region: [null, [Validators.nullValidator]],
        doorNumber: [null, [Validators.nullValidator]],
        emergencyNumber: [null, [Validators.nullValidator]],
      }),
      contactData: this.fb.group({
        phoneNumber: [null, [Validators.required]],
        homeNumber: [null, [Validators.nullValidator]],
        workNumber: [null, [Validators.nullValidator]],
        email: [null, [Validators.nullValidator]],
      })
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
      this.router.navigate(["/muslime/family", this.Encryption.encryptUsingAES256(response)])

    }
  }
  override postSubscribtion(): void { }
  override returnGetModelByIdFn(): Observable<any> {
    return this.swagger.apiMuslimeGetContactDataGet(this.url)
  }
  override returnEditFn(): Observable<any> {
    this.form.get('muslimeId')?.patchValue(this.url)
    return this.swagger.apiMuslimeCreateContactAndInfoDataPost(this.form.value);
  }



  showDialog() {
    this.display = true;
  }

  CancelDialog() {
    this.display = false;
  }

  returnAddFn(): Observable<any> {
    this.form.get('muslimeId')?.patchValue(this.url)
    return this.swagger.apiMuslimeCreateContactAndInfoDataPost(this.form.value);
  }

}
