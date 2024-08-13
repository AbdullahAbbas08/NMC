import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-muslime-index',
  templateUrl: './muslime-index.component.html',
  styleUrls: ['./muslime-index.component.scss']
})
export class MuslimeIndexComponent implements OnInit {
  State: number = 0
  param: any
  constructor(private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {

  }


  isRouteActive(routeName: string): boolean {
    this.route.firstChild?.data.subscribe(data => {
      this.State = parseInt(data['number']);
    });
    return this.router.isActive(routeName, false);
  }




}
