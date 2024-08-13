import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SwaggerClient, Witness } from '../../../../../Shared/Services/Swagger/SwaggerClient.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-witness-form',
  templateUrl: './witness-form.component.html',
  styleUrls: ['./witness-form.component.scss']
})
export class WitnessFormComponent implements OnInit {
  @Output() display = new EventEmitter<any>();
  model: Witness;
  constructor() { }

  ngOnInit(): void {
    this.model = new Witness()
  }



}
