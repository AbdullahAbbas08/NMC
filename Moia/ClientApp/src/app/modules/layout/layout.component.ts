import { Component, OnInit } from '@angular/core';
import { ResizedEvent } from 'angular-resize-event/public-api';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {
  headerHeight: number;
  constructor() { }

  ngOnInit(): void {
  }

  onHeaderResized(event: ResizedEvent) {
    this.headerHeight = event.newRect.height;
  }

}
