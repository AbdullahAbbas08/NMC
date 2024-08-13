import { HttpErrorResponse } from '@angular/common/http';
import { Directive, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Location } from '@angular/common';
import { MessageService } from 'primeng/api';

@Directive()
export abstract class DefaultFormComponent<K> implements OnInit {
  isLoadingData: boolean = false;
  isLoadingBtn: boolean = false;
  form!: FormGroup;
  editMode: boolean = false;
  constructor(
    protected route: ActivatedRoute,
    protected fb: FormBuilder,
    protected router: Router,
    protected location: Location,
    protected toast: MessageService
  ) { }

  ngOnInit(): void {
    this.initForm();
    this.checkAddEdit();
    this.getDataFromServer();
  }

  public checkAddEdit() {
    if (this.route.snapshot.paramMap.get('id')) {
      this.editMode = true;
    }
  }
  Back() {
    this.location.back()
  }

  public getDataFromServer() {
    this.isLoadingData = true;
    if (this.editMode) {
      // this.breadcrumb.push({
      //   label: this.translate.instant('Edit'),
      // });
      this.editMode = true;
      this.onEdit();
      this.returnGetModelByIdFn().subscribe(
        (entity) => {
          this.postSubscribtion();
          this.form.patchValue(entity as any);
          this.isLoadingData = false;
          
        },
        (err: HttpErrorResponse) => {
          this.isLoadingData = false;
        }
      );
    } else {
      // this.breadcrumb.push({
      //   label: this.translate.instant('Add'),
      // });
      this.onAdd();
      this.isLoadingData = false;
    }
  }

  submit() {
    if (this.form.invalid) {

      this.form.markAllAsTouched();
    } else {
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

  onCancel(): void {
    // this.router.navigate([
    //   this.breadcrumb[this.breadcrumb.length - 2].routerLink, ]);
  }

  // Encrypt(id: any): any {
  //   let val = this.auth.encryptUsingAES256(id?.toString());
  //   let NewVal = val.replace(/\//g, "__")
  //   return NewVal;
  // }

  abstract initForm(): void;
  abstract onAdd(): void;
  abstract onEdit(): void;
  abstract onSave(response: any): void;
  abstract postSubscribtion(): void;
  abstract returnGetModelByIdFn(): Observable<K>;
  // abstract returnAddFn(): Observable<boolean>;
  abstract returnAddFn(): Observable<any>;
  // abstract returnEditFn(): Observable<boolean>;
  abstract returnEditFn(): Observable<any>;

  validateEnglishChars(event: KeyboardEvent): boolean {
    const pattern = /^[A-Za-z0-9\s]*$/; // Regular expression to match English characters, numbers, and spaces
    const inputChar = String.fromCharCode(event.keyCode);
    if (!pattern.test(inputChar)) {
      event.preventDefault(); // Prevent the keypress event if the character is not English or a number
      return false;
    }

    return true;
  }

  validateArabicChars(event: KeyboardEvent): boolean {
    const pattern = /^[\u0600-\u06FF0-9\s]*$/; // Regular expression to match Arabic characters, numbers, and spaces
    const inputChar = String.fromCharCode(event.keyCode);

    if (!pattern.test(inputChar)) {
      event.preventDefault(); // Prevent the keypress event if the character is not Arabic or a number
      return false;
    }
    return true;
  }

  validateNumericInput(event: KeyboardEvent): boolean {
    const keyCode = event.keyCode || event.which;
    const charCode = String.fromCharCode(keyCode);

    if (!/^\d+$/.test(charCode)) {
      event.preventDefault(); // Prevent the keypress event if the character is not a number
      return false;
    }

    return true;
  }

}
