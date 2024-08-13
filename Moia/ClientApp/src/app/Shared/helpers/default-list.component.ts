import { HttpErrorResponse } from '@angular/common/http';
import { Directive, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable, debounceTime, distinctUntilChanged, switchMap, tap } from 'rxjs';
import { EncryptDecryptService } from '../Services/encrypt-decrypt.service';
import { ConfirmationService, MessageService } from 'primeng/api';

@Directive()
export abstract class DefaultListComponent<
  T extends {
    paginationList?: K[] | undefined;
    originalListListCount?: number;
  },
  K
> implements OnInit {
  entities: K[] | undefined = [];
  isLoading: boolean = false;
  page: number = 1;
  pageSize: number = 10;
  count: number = 0;
  pageSizeOptions: number[] = [10, 20, 50];
  searchTermControl: FormControl = new FormControl('');
  constructor(
    protected router: Router,
    protected auth: EncryptDecryptService,
    protected confirmationService: ConfirmationService,
    protected toast: MessageService,
  ) { }

  ngOnInit(): void {
    this.getData();
    this.trackSearchTerm();
  }

  EncryptId(id: any): any {
    let val = this.auth.encryptUsingAES256(id?.toString());
    let NewVal = val.replace(/\//g, "__")
    return NewVal;
  }

  getData($event?: any) {
    this.isLoading = true;
    if ($event) {
      this.page = $event.page + 1;
      this.pageSize = $event.rows;
    }
    this.returnDataFn().subscribe(
      (res) => {
        this.entities = res.paginationList;
        
        this.count = res.originalListListCount ? res.originalListListCount : 0;
        this.isLoading = false;

      },
      (err: HttpErrorResponse) => {
        this.isLoading = false;
      }
    );
  }

  trackSearchTerm() {
    this.searchTermControl.valueChanges
      .pipe(
        tap(() => {
          this.isLoading = true;
        }),
        debounceTime(1000),
        distinctUntilChanged(),
        switchMap(() => {
          return this.returnDataFn();
        })
      )
      .subscribe(
        (res) => {
          this.entities = res.paginationList;
          this.count = res.originalListListCount
            ? res.originalListListCount
            : 0;
          this.isLoading = false;
        },
        (err: HttpErrorResponse) => {
          this.isLoading = false;
        }
      );
  }

  delete(id: number) {
    this.confirmationService.confirm({
      message: 'تأكيد عملية الحذف',
      key: 'deleteConfirm',
      accept: () => {
        this.isLoading = true;
        this.returnDeleteFn(id).subscribe(
          (res) => {
            this.toast.add({
              severity: 'success',
              detail: 'بيسب',
            });
            this.isLoading = false;
            this.getData();
          },
          (err: HttpErrorResponse) => {
            this.isLoading = false;
          }
        );
      },
    });
  }

  Confirm(id: number) {
    this.confirmationService.confirm({
      message: 'هل أنت متأكد من إرسال الطلب ؟',
      key: 'SendConfirm',
      accept: () => {
        this.isLoading = true;
        this.returnDeleteFn(id).subscribe(
          (res) => {
            this.toast.add({
              severity: 'success',
              detail: 'بيسب',
            });
            this.isLoading = false;
            this.getData();
          },
          (err: HttpErrorResponse) => {
            this.isLoading = false;
          }
        );
      },
    });
  }

  add() {
    this.router.navigate([this.router.url + '/form/']);
  }

  edit(id: number) {
    this.router.navigate([this.router.url + '/form/' + id]);
  }

  entityType(entity: K): K {
    return entity;
  }

  validateEnglishChars(event: KeyboardEvent): boolean {
    const pattern = /^[A-Za-z0-9\s]*$/; // Regular expression to match English characters, numbers, and spaces
    const inputChar = String.fromCharCode(event.keyCode);
    if (!pattern.test(inputChar)) {
      event.preventDefault(); // Prevent the keypress event if the character is not English or a number
      return false;
    }

    return true;
  }

  validateEnglish_No_special_Chars(event: KeyboardEvent): boolean {
    const pattern = /^[A-Za-z0-9\s!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?`~]*$/; // Regular expression to match English characters, numbers, spaces, and special characters
    const inputChar = String.fromCharCode(event.keyCode);

    if (!pattern.test(inputChar)) {
      event.preventDefault(); // Prevent the keypress event if the character does not match the allowed pattern
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

  abstract returnDataFn(): Observable<T>;

  abstract returnDeleteFn(id: number): Observable<boolean>;
}
