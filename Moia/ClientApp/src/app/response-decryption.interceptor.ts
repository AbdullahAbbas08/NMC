import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, mergeMap, tap } from 'rxjs/operators';
import { BrowserStorageService } from './Shared/Services/browser-storage.service';

@Injectable()
export class ResponseDecryptionInterceptor implements HttpInterceptor {

  IncludeURLList: string[] = [
    "api/Committee",
    "api/Witness",
    "api/Lookup",
    "api/Muslime",
     "api/User",
  ];

  constructor(private encryptionService: BrowserStorageService) { }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<any> {
    if (this.shouldIntercept(req.url)) {
      return next.handle(req).pipe(
        mergeMap(async event => {
          // debugger
          if (event instanceof HttpResponse) {
            try {
              const originalEvent = event.clone();
              let ParsedValue = null
              if (event.body instanceof Blob) {
                let modifiedData = await this.handleBlobResponse(event);
                ParsedValue = JSON.parse(modifiedData);
                const EncryptedValue = JSON.parse(this.decryptData(ParsedValue.bDlTwThDpe8K));
                const _jsonBlob = new Blob([JSON.stringify(EncryptedValue)], { type: 'application/json' });

                const modifiedResponse = new HttpResponse<any>({
                  body: _jsonBlob,
                  status: originalEvent.status,
                  statusText: originalEvent.statusText,
                  headers: originalEvent.headers,
                  url: originalEvent.url!,
                });
                return modifiedResponse;
              }
              else {
                let modifiedData = event.body
                ParsedValue = modifiedData;
                const EncryptedValue = JSON.parse(this.decryptData(ParsedValue.bDlTwThDpe8K));
                // const _jsonBlob = new Blob([JSON.stringify(EncryptedValue)], { type: 'application/json' });

                const modifiedResponse = new HttpResponse<any>({
                  body: EncryptedValue,
                  status: originalEvent.status,
                  statusText: originalEvent.statusText,
                  headers: originalEvent.headers,
                  url: originalEvent.url!,
                });
                return modifiedResponse;
              }


            }
            catch (error) {
              return event;
            }

          } else {
            return event;
          }
        })
      );
    } else {
      return next.handle(req);
    }
  }



  private async handleBlobResponse(response: any): Promise<any> {
    const blobData = response.body;
    if (blobData) {
      const textData = await this.blobToText(blobData);
      const modifiedData = textData;
      return modifiedData;
    } else {
      return null;
    }
  }


  private blobToText(blob: Blob): Promise<string> {
    return new Promise<string>((resolve, reject) => {
      const reader = new FileReader();

      reader.onload = (event) => {
        const result = event?.target?.result;
        if (result) {
          resolve(result as string);
        } else {
          reject('Unable');
        }
      };

      reader.onerror = (event) => {
        reject(event);
      };

      reader.readAsText(blob);
    });
  }


  private shouldIntercept(url: string): boolean {
    return this.IncludeURLList.some(element => url.includes(element));
  }

  private decryptData(data: string): any {
    return this.encryptionService.decrypteString(data);
  }

}
