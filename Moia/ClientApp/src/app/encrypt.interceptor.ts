import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { EncryptDecryptService } from './Shared/Services/encrypt-decrypt.service';
import { DefaultInterceptor } from './_encryptData.class';
@Injectable()
export class EncryptInterceptor extends DefaultInterceptor implements HttpInterceptor {

  constructor(protected override encryptDecryptService: EncryptDecryptService) {
    super(encryptDecryptService)
  }

  IncludeURLList = [
    "api/Committee",
    "api/Witness",
    "api/Lookup",
    "api/Muslime",
    "api/User",
  ];

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let InludeFound = this.IncludeURLList.filter(element => {
      return req.url.includes(element)
    }
    );
    // We have Encrypt the GET and POST call before pass payload to API
    if ((InludeFound && InludeFound.length > 0)) {
      return next.handle(this.handleApiCall(req));
    }
    return next.handle(req);
  }
}
