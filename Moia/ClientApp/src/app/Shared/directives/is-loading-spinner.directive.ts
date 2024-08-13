import { DOCUMENT } from '@angular/common';
import { ChangeDetectorRef, Directive, ElementRef, Inject, Input, Renderer2 } from '@angular/core';

@Directive({
  selector: '[IsLoadingSpinner]'
})
export class IsLoadingSpinnerDirective {

  @Input('IsLoadingSpinner') IsLoadingSpinner: boolean = false;
  child: HTMLElement | null = null;
  constructor(
    private elementRef: ElementRef,
    private renderer: Renderer2,
    @Inject(DOCUMENT) private document: Document,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnChanges() {
    if (this.IsLoadingSpinner) {
      this.elementRef.nativeElement.disabled = true;
      this.cdr.detectChanges();
      const fontSize = this.elementRef.nativeElement.offsetHeight / 1.3;
      this.child = this.document.createElement('div');
      this.renderer.addClass(
        this.elementRef.nativeElement,
        'spinner-overlay-container'
      );
      this.renderer.addClass(this.child, 'spinner-overlay');
      this.child.innerHTML = `<i class="pi pi-spin pi-spinner site-color-1" style="font-size: ${
        fontSize < 60 ? fontSize : 60
      }px"></i>`;
      this.renderer.appendChild(this.elementRef.nativeElement, this.child);
    } else {
      this.elementRef.nativeElement.disabled = false;
      this.renderer.removeClass(
        this.elementRef.nativeElement,
        'spinner-overlay-container'
      );
      this.child &&
        this.renderer.removeChild(this.elementRef.nativeElement, this.child);
    }
  }

}
