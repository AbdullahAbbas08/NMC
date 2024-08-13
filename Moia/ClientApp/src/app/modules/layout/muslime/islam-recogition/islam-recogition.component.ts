import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DatePipe, Location } from '@angular/common';
import { DefaultFormComponent } from 'src/app/Shared/helpers/default-form.component';
import { FormBuilder, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { IslamRecognitionWayDto, IsslamRecognition, PersonalDataDto, SwaggerClient } from 'src/app/Shared/Services/Swagger/SwaggerClient.service';
import { EncryptDecryptService } from 'src/app/Shared/Services/encrypt-decrypt.service';
import { Observable } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-islam-recogition',
  templateUrl: './islam-recogition.component.html',
  styleUrls: ['./islam-recogition.component.scss']
})
export class IslamRecogitionComponent extends DefaultFormComponent<PersonalDataDto> {
  url: any = this.route.snapshot.paramMap.get('id')
    ? this.Encryption.decryptUsingAES256(this.route.snapshot.paramMap.get('id')?.replace(/__/g, "/")) : 0

  date: any
  users: any[]
  selectedCity: any
  display: boolean = false;
  IslamRecognitionWays: IsslamRecognition[] = []
  IslamRecognitionWaysData: IsslamRecognition[] = []
  selectedItems: IsslamRecognition[] = [];
  _selectedItems: IsslamRecognition[] = [];

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
    this.RecognitionWays()
    

  }

  override initForm(): void {

    this.form = this.fb.group({
      muslimeId: [this.route.snapshot.paramMap.get('id') ? this.url : 0, [Validators.required]],
      nameBeforeFristAr: [null, [Validators.required]],
      nameBeforeMiddleAr: [null, [Validators.required]],
      nameBeforeLastAr: [null, [Validators.required]],
      nameAfter: [null, [Validators.required]],
      islamDate: [null, [Validators.required]],
      islamDateHijry: [null, [Validators.required]],
      firstWitness: [null, [Validators.required]],
      secondWitness: [null, [Validators.required]],
      preacherName: [null, [Validators.required]],
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
      this.router.navigate(["/muslime/attachment", this.Encryption.encryptUsingAES256(response)])

    }
  }
  override postSubscribtion(): void {

  }
  override returnGetModelByIdFn(): Observable<any> {
    this.swagger.apiMuslimeGetIslamRecognitionWayGet(this.url).subscribe(
      res => {
        if (res) {
          this._selectedItems = res
          this.selectedItems = this._selectedItems
        }
      }
    )
    return this.swagger.apiMuslimeGetIslamRecognitionWayGet(this.url)
  }

  ExistWay(entity: IsslamRecognition): boolean {
    return this.IslamRecognitionWaysData.some(x => x.title == entity.title)
  }

  override returnEditFn(): Observable<any> {

    this.form.get('muslimeId')?.patchValue(this.url)
    return this.swagger.apiMuslimeCreateIslamRecognitionWaysPost(
      new IslamRecognitionWayDto({
        muslimeId: this.url,
        islamRecognitionWay: this.selectedItems
      })
    );

  }

  RecognitionWays() {
    this.swagger.apiLookupRecognitionWaysGet().subscribe(
      res => {
        this.IslamRecognitionWays = res
        // console.log("... ", this.IslamRecognitionWays);


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
    this.form.get('muslimeId')?.patchValue(this.url)
    return this.swagger.apiMuslimeCreateIslamRecognitionWaysPost(
      new IslamRecognitionWayDto({
        muslimeId: this.url,
        islamRecognitionWay: this.selectedItems
      })
    );
  }


  override submit() {

    this.isLoadingBtn = true;
    if (this.editMode) {
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
    } else {
      this.returnAddFn().subscribe(
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
  }

}

