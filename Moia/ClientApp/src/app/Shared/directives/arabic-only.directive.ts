import { Directive, HostListener } from '@angular/core';

@Directive({
  selector: '[arabicOnly]'
})
export class ArabicOnlyDirective {

  @HostListener('keypress', ['$event'])
  onKeyDown(event: KeyboardEvent) {
    const pattern = /^[^\u0600-\u06FF]+$/; // Regular expression to match non-Arabic characters

    // Check if the entered key matches the pattern
    const inputChar = String.fromCharCode(event.keyCode);
    if (!pattern.test(inputChar)) {
      event.preventDefault(); // Prevent the keypress event if the character is not Arabic
    }
  }

}
