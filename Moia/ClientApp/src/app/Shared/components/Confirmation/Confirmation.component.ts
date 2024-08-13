import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-Confirmation',
  templateUrl: './Confirmation.component.html',
  styleUrls: ['./Confirmation.component.scss']
})
export class ConfirmationComponent implements OnInit {

  // display:boolean=false;
  @Input() display:any;
  @Input() isChecked:any;
  @Output() confirm = new EventEmitter<any>();
  @Output() open = new EventEmitter<any>();
  @Output() close = new EventEmitter<any>();
  constructor() { }

  ngOnInit() {
  }
  // isChecked: boolean = false;
  _confirm(){
    this.confirm.next(true)
  }
 
  cancel(){
    this.isChecked = false
    this.close.next(true)
  }
}
