import { HttpRequest } from "@angular/common/http"

export const handleAptFormHttpClient =(req:HttpRequest<any>,encryptDecryptService:any):HttpRequest<any>=>{

    const _url =req.url
    if (_url.indexOf("?") > 0) {
        let encriptURL =handleGetUrlStringOnly(req.url,encryptDecryptService);
        const cloneReq = req.clone({
          url: encriptURL
        });
        return cloneReq;
    }
    return req;
}

//this helper func to convert url to encrepted
export const handleGetUrlStringOnly =(url:string,encryptDecryptService:any)=>{
  // debugger
  let data = url.indexOf('?')? url.substr(0, url.indexOf("?") + 1) + encryptDecryptService.encryptUsingAES256(url.substr(url.indexOf("?") + 1, url.length)):url
  //decrypt(data,encryptDecryptService);
  return data
}

export const decrypt = (url:string,encryptDecryptService:any)=>{
  const _url =url.indexOf('?')? url.substr(0, url.indexOf("?") + 1) + encryptDecryptService.decryptUsingAES256(url.substr(url.indexOf("?") + 1, url.length)):url
  // console.log(_url);
  return _url
}


// method : post
// body encrpt 
export const handleApiFormHttpClientWithBodyPost=(req:HttpRequest<any>,encryptDecryptService:any):HttpRequest<any> =>{
        let EncriptPostURL = handleGetUrlStringOnly(req.url,encryptDecryptService);
        if (req.body.length > 0) {
          
          if (req.url.indexOf("?") > 0) {
            const cloneReq = req.clone({
              url: EncriptPostURL,
              body: encryptDecryptService.encryptUsingAES256(req.body),
            });
            return cloneReq;
          } else {
            const cloneReq = req.clone({
              body: encryptDecryptService.encryptUsingAES256(req.body),
            });
            return cloneReq;
          }
        }else{
          return req
        }

}

// method : post
// url encrpt
export const handleApiFormHttpClientWithUrlPost =(req:HttpRequest<any>,encryptDecryptService:any):HttpRequest<any>=>{
    let EncriptPostURL = handleGetUrlStringOnly(req.url,encryptDecryptService);
    const cloneReq = req.clone({
      body: null,
      url: EncriptPostURL
    });
    return cloneReq
}