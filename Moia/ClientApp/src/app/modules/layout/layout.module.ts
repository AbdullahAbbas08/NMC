import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { LayoutComponent } from './layout.component';
import { LayoutRoutingModule } from './layout-routing.module';

import { SidebarComponent } from './sidebar/sidebar.component';
import { HeaderComponent } from './header/header.component';
import { AngularResizeEventModule } from 'angular-resize-event';
import { MuslimeModule } from './muslime/muslime.module';
import { SharedModule } from 'src/app/Shared/shared.module';
import { SettingsComponent } from './settings/settings.component';
import { ProfileComponent } from './profile/profile.component';



@NgModule({
  declarations: [
    LayoutComponent,
    SidebarComponent,
    HeaderComponent,
    SettingsComponent,
    ProfileComponent
    
  ],
  imports: [
    CommonModule,
    LayoutRoutingModule,
    AngularResizeEventModule,
    MuslimeModule,
    SharedModule
  ],
  providers: [DatePipe],
})
export class LayoutModule { }
