import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PersonalDataComponent } from './personal-data/personal-data.component';
import { MuslimeIndexComponent } from './muslime-index.component';
import { PersonalInformationComponent } from './personal-information/personal-information.component';
import { ContactAddressDataComponent } from './contact-address-data/contact-address-data.component';
import { FamilyWorkDataComponent } from './family-work-data/family-work-data.component';
import { IslamRecogitionComponent } from './islam-recogition/islam-recogition.component';
import { AttachmentComponent } from './attachment/attachment.component';
import { ConfirmRequestComponent } from './confirm-request/confirm-request.component';

const routes: Routes = [
  {
    path: '', component: MuslimeIndexComponent,
    children: [
      { path: '', component: PersonalDataComponent, pathMatch: 'full', data: { number: '1' } },
      { path: 'personal-data/:id', component: PersonalDataComponent, pathMatch: 'full', data: { number: '1' } },
      { path: 'personal-information/:id', component: PersonalInformationComponent, pathMatch: 'full', data: { number: '2' } },
      { path: 'contact-address/:id', component: ContactAddressDataComponent, pathMatch: 'full', data: { number: '3' } },
      { path: 'contact-address', component: ContactAddressDataComponent, pathMatch: 'full', data: { number: '3' } },
      { path: 'family/:id', component: FamilyWorkDataComponent, pathMatch: 'full', data: { number: '4' } },
      { path: 'islam/:id', component: IslamRecogitionComponent, pathMatch: 'full', data: { number: '5' } },
      { path: 'attachment/:id', component: AttachmentComponent, pathMatch: 'full', data: { number: '6' } },
      { path: 'attachment', component: AttachmentComponent, pathMatch: 'full', data: { number: '6' } },
      { path: 'confirm-request/:id', component: ConfirmRequestComponent, pathMatch: 'full', data: { number: '7' } },


    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MuslimeRoutingModule { }
