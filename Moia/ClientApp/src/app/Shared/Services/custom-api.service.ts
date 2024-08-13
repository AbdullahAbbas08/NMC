import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { API_BASE_URL } from './Swagger/SwaggerClient.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CustomApiService {

  baseUrl: string;
  constructor(private http: HttpClient, @Inject(API_BASE_URL) baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  apiMuslimeInsertAttachment(data: FormData): Observable<any> {
    return this.http.post(
      this.baseUrl + `/api/Muslime/InsertAttachment`,
      data,
    );
  }

  apiMuslimeSendOrder(OrderCode: any, data: FormData): Observable<any> {
    return this.http.post(
      this.baseUrl + `/api/Muslime/SendOrder?OrderCode=${OrderCode}`,
      data,
    );
  }

  apiUserAddSignature(data: FormData): Observable<any> {
    return this.http.post(
      this.baseUrl + `api/User/InsertUserSignature`,
      data,
    );
  }

  SaveUser(data: FormData): Observable<any> {
    return this.http.post(
      this.baseUrl + `/api/User/AddUpdateUser`,
      data,
    );
  }
}
