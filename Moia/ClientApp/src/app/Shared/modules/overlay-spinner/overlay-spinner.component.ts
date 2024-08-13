import { Component, OnInit } from '@angular/core';
import { OverlaySpinnerService } from './overlay-spinner.service';

@Component({
  selector: 'app-overlay-spinner',
  templateUrl: './overlay-spinner.component.html',
  styleUrls: ['./overlay-spinner.component.scss']
})
export class OverlaySpinnerComponent implements OnInit {

  constructor(public overlaySpinnerSvr: OverlaySpinnerService) {}

  ngOnInit(): void {
  }

}
