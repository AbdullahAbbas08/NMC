import {
    HttpRequest,
  } from '@angular/common/http';

import { handleApiFormHttpClientWithBodyPost, handleApiFormHttpClientWithUrlPost, handleAptFormHttpClient } from './_encryptData';
import { EncryptDecryptService } from './Shared/Services/encrypt-decrypt.service';


export abstract class DefaultInterceptor {

    constructor(protected encryptDecryptService: EncryptDecryptService) { }

    handleGetRequest(req:HttpRequest<any>):HttpRequest<any>{
        return handleAptFormHttpClient(req,this.encryptDecryptService);
    }

    handlePostRequestWithBody(req:HttpRequest<any>):HttpRequest<any>{
        return handleApiFormHttpClientWithBodyPost(req,this.encryptDecryptService)
    }

    handlePostRequestWithUrl(req:HttpRequest<any>):HttpRequest<any>{

        return handleApiFormHttpClientWithUrlPost(req,this.encryptDecryptService);
    }


    handleApiCall(req:HttpRequest<any>):HttpRequest<any>{
        if (req.method == "GET") {
            return this.handleGetRequest(req);
          } else if (req.method == "POST") {
            if (!(req.body instanceof FormData)) {
              if (req.body !== null) {
                return this.handlePostRequestWithBody(req);
              }
               else if (req.url.indexOf("?") > 0) {
               
                return this.handlePostRequestWithUrl(req);
              } else { }
              return req;
            }
          }
        return req
    }
}