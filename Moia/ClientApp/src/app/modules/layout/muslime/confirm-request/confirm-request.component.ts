import { DatePipe,Location } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import { MessageService } from "primeng/api";
import { Observable } from "rxjs";
import { DefaultFormComponent } from "src/app/Shared/helpers/default-form.component";
import { AuthService } from "src/app/Shared/Services/auth.service";
import { EncryptDecryptService } from "src/app/Shared/Services/encrypt-decrypt.service";
import { OrderStage, OrderStatus, SwaggerClient } from "src/app/Shared/Services/Swagger/SwaggerClient.service";




@Component({
    selector: 'app-confirm-request',
    templateUrl: './confirm-request.component.html',
    styleUrls: ['./confirm-request.component.scss']
  })
  export class ConfirmRequestComponent implements OnInit  {

    isChecked = false;

    formGroup: FormGroup;
    isLoadingBtn: boolean = false;
    orderNo:any



    url: any = this.route.snapshot.paramMap.get('id')
    ? this.Encryption.decryptUsingAES256(this.route.snapshot.paramMap.get('id')?.replace(/__/g, "/")) : 0
  



  constructor(
    private route: ActivatedRoute,
    private router:Router,
    private location: Location,
    private toast: MessageService,
    private swagger: SwaggerClient,
    private Encryption: EncryptDecryptService,
    private translate: TranslateService,private authService:AuthService) {
  }


   ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      this.orderNo=this.Encryption.decryptUsingAES256(id)   
    })

    this.formGroup = new FormGroup({
            confirm: new FormControl(this.isChecked)
        });

  }

  submit(){
 this.swagger.apiMuslimeChangeOrderStatePost(this.orderNo, OrderStatus.Send, OrderStage.Committee,
        this.translate.instant('SendOrderToCommitteeDescription') + " " + "بواسطة : " + this.authService.User$.value['Name']).subscribe(
          res => {
            if (res == null) {
              this.toast.add({
                severity: 'error',
                detail: 'لم تتم العملية بنجاح',
              });
            } else {
              this.router.navigateByUrl("/order")
            }
          }
        )
  }

  Save() {
   
        this.router.navigateByUrl("/order")
      }

  toggleConfirm(){
  this.isChecked=!this.isChecked
  
}

  
  


  Back(){
    this.location.back()


  }
  


  
  }