import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DatePipe, Location } from '@angular/common';
import { DefaultFormComponent } from 'src/app/Shared/helpers/default-form.component';
import { FamilyAndWorkDto, SwaggerClient } from 'src/app/Shared/Services/Swagger/SwaggerClient.service';
import { FormBuilder, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { EncryptDecryptService } from 'src/app/Shared/Services/encrypt-decrypt.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-family-work-data',
  templateUrl: './family-work-data.component.html',
  styleUrls: ['./family-work-data.component.scss']
})
export class FamilyWorkDataComponent extends DefaultFormComponent<FamilyAndWorkDto> {
  url: any = this.route.snapshot.paramMap.get('id')
    ? this.Encryption.decryptUsingAES256(this.route.snapshot.paramMap.get('id')?.replace(/__/g, "/")) : 0

  date: any
  users: any[]
  selectedCity: any
  display: boolean = false;

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
  }

  override ngOnInit(): void {
    this.initForm();
    this.checkAddEdit();
    this.getDataFromServer();

  }

  override initForm(): void {

    this.form = this.fb.group({
      muslimeId: [this.url, [Validators.nullValidator]],
      familyInformation: this.fb.group({
        membersNumber: [null, [Validators.nullValidator]],
        boysNumber: [null, [Validators.nullValidator]],
        girlsNumber: [null, [Validators.nullValidator]],
      }),
      work: this.fb.group({
        profession: [null, [Validators.required]],
        companyTitle: [null, [Validators.nullValidator]],
        directManager: [null, [Validators.nullValidator]],
        city: [null, [Validators.nullValidator]],
        street: [null, [Validators.nullValidator]],
        address: [null, [Validators.nullValidator]],
        postalBox: [null, [Validators.nullValidator]],
        postalCode: [null, [Validators.nullValidator]],
      }),
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
      this.router.navigate(["/muslime/islam", this.Encryption.encryptUsingAES256(response)])

    }
  }
  override postSubscribtion(): void { }
  override returnGetModelByIdFn(): Observable<any> {
    return this.swagger.apiMuslimeGetFamilyAndWorkGet(this.url)
  }
  override returnEditFn(): Observable<any> {
    this.form.get('muslimeId')?.patchValue(this.url)
    return this.swagger.apiMuslimeCreateFamilyAndWorkDtoPost(this.form.value);
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
    this.form.get('muslimeId')?.patchValue(this.url)
    return this.swagger.apiMuslimeCreateFamilyAndWorkDtoPost(this.form.value);
  }


}
