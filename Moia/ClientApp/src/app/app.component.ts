import { DOCUMENT } from '@angular/common';
import { Component, Inject, Renderer2 } from '@angular/core';
import { RouteConfigLoadEnd, RouteConfigLoadStart, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { OverlaySpinnerService } from './Shared/modules/overlay-spinner/overlay-spinner.service';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'بطاقة مسلم جديد';
  constructor(
    public translate: TranslateService,
    @Inject(DOCUMENT) private document: Document,
    private renderer: Renderer2,
    private router: Router,
    private overlaySpinnerSvr: OverlaySpinnerService,
    private titleService: Title
  ) {
  }
  ngOnInit(): void {
    this.setTitle(this.translate.instant('appTitle'));
  }

  setTitle(newTitle: string) {
    this.titleService.setTitle(newTitle);
  }

  initStyle(lang: string) {
    if (lang === 'ar') {
      this.renderer.addClass(this.document.body, 'rtl');
    } else {
      this.renderer.removeClass(this.document.body, 'rtl');
    }
  }

  onLangChange() {
    this.translate.onLangChange.subscribe((lang: any) => {
      this.initStyle(lang.lang);
    });
  }

  showOverlaySpinnerOnLazyLoading() {
    this.router.events.subscribe((event) => {
      if (event instanceof RouteConfigLoadStart) {
        this.overlaySpinnerSvr.show();
      } else if (event instanceof RouteConfigLoadEnd) {
        this.overlaySpinnerSvr.hide();
      }
    });
  }
}
