import { Directive, HostListener, ElementRef } from '@angular/core';

@Directive({
  selector: '[englishOnly]'
})
export class EnglishOnlyDirective {

  constructor(private el: ElementRef) { }

  @HostListener('input', ['$event'])
  onInput(event: Event) {
    const input = this.el.nativeElement as HTMLInputElement;
    const englishOnlyValue = input.value.replace(/[^A-Za-z ]/g, '');
    input.value = englishOnlyValue;
  }
}
