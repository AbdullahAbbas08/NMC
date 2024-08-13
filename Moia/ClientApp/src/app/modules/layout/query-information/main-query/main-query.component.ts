import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SwaggerClient, UserDataForVieweing } from 'src/app/Shared/Services/Swagger/SwaggerClient.service';

@Component({
  selector: 'app-main-query',
  templateUrl: './main-query.component.html',
  styleUrls: ['./main-query.component.scss']
})
export class MainQueryComponent implements OnInit {
  UserData: UserDataForVieweing
  constructor(private swagger: SwaggerClient, private route: ActivatedRoute) { }
  UserId: any
  ngOnInit(): void {
    const param1Value = this.route.snapshot.paramMap.get('id')
    this.swagger.apiMuslimeGetDataForQueryPost(param1Value).subscribe(
      res => {
        this.UserData = res
      }
    )
  }
}
