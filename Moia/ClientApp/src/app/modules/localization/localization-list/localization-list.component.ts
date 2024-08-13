import { Component, OnInit } from '@angular/core';
import { LocalizationDetailsDTO } from 'src/app/Shared/Services/Swagger/SwaggerClient.service';
import { DefaultListComponent } from 'src/app/Shared/helpers/default-list.component';

@Component({
  selector: 'app-localization-list',
  templateUrl: './localization-list.component.html',
  styleUrls: ['./localization-list.component.scss']
})
export class LocalizationListComponent extends DefaultListComponent<
ViewerPaginationOfLocalizationDetailsDTO,
LocalizationDetailsDTO
> {

  constructor() { }

  ngOnInit(): void {
  }

}
