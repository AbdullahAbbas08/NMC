import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import {  IsslamRecognition, SwaggerClient } from 'src/app/Shared/Services/Swagger/SwaggerClient.service';
import { EncryptDecryptService } from 'src/app/Shared/Services/encrypt-decrypt.service';
import { DefaultFormComponent } from 'src/app/Shared/helpers/default-form.component';
import { Location } from '@angular/common';
import { Observable } from 'rxjs';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.scss']
})
export class FormComponent extends DefaultFormComponent<IsslamRecognition> {

  url: any = this.route.snapshot.paramMap.get('id')
    ? this.encryption.decryptUsingAES256(this.route.snapshot.paramMap.get('id')?.replace(/__/g, "/")) : 0

  constructor(
    route: ActivatedRoute,
    fb: FormBuilder,
    router: Router,
    location: Location,
    toast: MessageService,
    private swagger: SwaggerClient,
    private encryption: EncryptDecryptService,
  ) {
    super(route, fb, router, location, toast);
  }

  initForm(): void {

    this.form = this.fb.group({
      id: [this.url, [Validators.required]],
      title: [null, [Validators.required]],
    });
  }


  returnGetModelByIdFn(): Observable<IsslamRecognition> {
    return this.swagger.apiIslamRecognitionWayGetByIdGet(this.url);
  }

  returnAddFn(): Observable<any> {
    return this.swagger.apiIslamRecognitionWayRecognitionWayPost(this.form.value);
  }

  returnEditFn(): Observable<any> {
    // this.form.patchValue({"id":this.route.snapshot.paramMap.get('id')})
    return this.swagger.apiIslamRecognitionWayRecognitionWayPost(
      this.form.value
    );
  }


  onAdd(): void { }
  onEdit(): void { }
  onSave(response: boolean): void {
    !response &&
      // this.toast.add({
      //   severity: 'error',
      //   detail: 'KeyExist',
      // });
      this.router.navigateByUrl("/")
  }
  postSubscribtion(): void { }




}
