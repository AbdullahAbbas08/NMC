import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from './auth.service';
import { SwaggerClient } from './Swagger/SwaggerClient.service';
import { OverlaySpinnerService } from '../modules/overlay-spinner/overlay-spinner.service';
import { BrowserStorageService } from './browser-storage.service';


@Injectable({
  providedIn: 'root',
})
export class AppInitService {
  lang:any = 'ar';
  constructor(
    private translate: TranslateService,
    private authSvr: AuthService,
    private swagger: SwaggerClient,
    private overlaySpinner: OverlaySpinnerService,
    private browser: BrowserStorageService
  ) { 

    if (!this.browser.getLang()) {
      this.lang = 'ar';
      this.browser.setLang(this.lang);
    } else {
      this.lang = this.browser.getLang();
    }
    if (this.browser.getLocalization() == null) {
      this.browser.setLocalization(this.lang)
    }
  }

  async Init() {
    return new Promise(async (resolve, reject) => {
      this.overlaySpinner.show();
      await this.authSvr.initAuth();
      const lang = 'ar';
      this.translate.setDefaultLang(lang);
      this.swagger.apiLocalizationJsonGet(lang).subscribe(
        (res) => {
          this.translate.setTranslation(lang, JSON.parse(res));
          this.translate.use(lang);
          resolve(true);
          this.overlaySpinner.hide();
        },
        (err: HttpErrorResponse) => {
          resolve(true);
          this.overlaySpinner.hide();
        }
      );
    });
  }
}
