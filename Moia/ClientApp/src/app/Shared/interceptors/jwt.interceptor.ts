import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../Services/auth.service';
import { BrowserStorageService } from '../Services/browser-storage.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(
    private authSvr: AuthService,
    private browserStorage: BrowserStorageService
  ) { }

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (this.authSvr.isAuthenticated$.getValue()) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${this.browserStorage.getToken()}`,
        },
      });
    }

    return next.handle(request);
  }
}
