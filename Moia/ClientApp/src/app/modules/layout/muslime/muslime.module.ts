import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MuslimeRoutingModule } from './muslime-routing.module';
import { PersonalDataComponent } from './personal-data/personal-data.component';
import { MuslimeIndexComponent } from './muslime-index.component';
import { ContactAddressDataComponent } from './contact-address-data/contact-address-data.component';
import { PersonalInformationComponent } from './personal-information/personal-information.component';
import { FamilyWorkDataComponent } from './family-work-data/family-work-data.component';
import { IslamRecogitionComponent } from './islam-recogition/islam-recogition.component';
import { AttachmentComponent } from './attachment/attachment.component';
import { SharedModule } from 'src/app/Shared/shared.module';
import { WitnessFormComponent } from '../settings/witness/witness-form/witness-form.component';
import { ConfirmRequestComponent } from './confirm-request/confirm-request.component';
import { DialogModule } from 'primeng/dialog';


@NgModule({
  declarations: [
    PersonalDataComponent,
    MuslimeIndexComponent,
    PersonalInformationComponent,
    ContactAddressDataComponent,
    FamilyWorkDataComponent,
    IslamRecogitionComponent,
    AttachmentComponent,
    ConfirmRequestComponent
    // WitnessFormComponent
  ],
  imports: [
    CommonModule,
    MuslimeRoutingModule,
    SharedModule,
    DialogModule
  ]
})
export class MuslimeModule { }
