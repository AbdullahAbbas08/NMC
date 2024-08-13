import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { OverlaySpinnerModule } from "./Shared/modules/overlay-spinner/overlay-spinner.module";
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { API_BASE_URL, SwaggerClient } from './Shared/Services/Swagger/SwaggerClient.service';
import { TranslateModule } from '@ngx-translate/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { AppInitService } from './Shared/Services/app-init.service';
import { JwtInterceptor } from './Shared/interceptors/jwt.interceptor';
import { ResponseDecryptionInterceptor } from './response-decryption.interceptor';
import { EncryptInterceptor } from './encrypt.interceptor';

export function initializeApp(appInitService: AppInitService) {
  return (): Promise<any> => {
    return appInitService.Init();
  };
}

@NgModule({
  declarations: [
    AppComponent,
  ],

  bootstrap: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    TranslateModule.forRoot(),
    OverlaySpinnerModule,
    BrowserAnimationsModule,
    ToastModule,
    ConfirmDialogModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ResponseDecryptionInterceptor,
      multi: true,
    },
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApp,
      deps: [AppInitService],
      multi: true,
    },
    // {
    //   provide: HTTP_INTERCEPTORS,
    //   useClass: EncryptInterceptor,
    //   multi: true,
    // },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true,
    },
    {
      provide: API_BASE_URL,
      useValue: '',
    },
    SwaggerClient,
    ConfirmationService,
    MessageService,
  ]
})
export class AppModule { }
