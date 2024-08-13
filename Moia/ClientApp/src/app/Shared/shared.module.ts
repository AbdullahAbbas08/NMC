import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PrintErrorModule } from './modules/print-error/print-error.module';
import { TranslateModule } from '@ngx-translate/core';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CardModule } from 'primeng/card';
import { PaginatorModule } from 'primeng/paginator';
import { SidebarModule } from 'primeng/sidebar';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { FieldsetModule } from 'primeng/fieldset';
import { InputMaskModule } from 'primeng/inputmask';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { DialogModule } from 'primeng/dialog';
import { SkeletonModule } from 'primeng/skeleton';
import { MultiSelectModule } from 'primeng/multiselect';
import { PasswordModule } from 'primeng/password';
import { InputSwitchModule } from 'primeng/inputswitch';
import { IsLoadingSpinnerModule } from './directives/is-loading-spinner.module';
import { HasPermissionDirective } from './directives/has-permission.directive';
import {MenuModule} from 'primeng/menu';
import {MegaMenuModule} from 'primeng/megamenu';
import {CheckboxModule} from 'primeng/checkbox';
import {TooltipModule} from 'primeng/tooltip';
import {AccordionModule} from 'primeng/accordion';
import {TimelineModule} from 'primeng/timeline';
import {FileUploadModule} from 'primeng/fileupload';
import { ConfirmationComponent } from './components/Confirmation/Confirmation.component';

const imports = [
  CommonModule,
  ToastModule,
  FormsModule,
  ReactiveFormsModule,
  PrintErrorModule,
  IsLoadingSpinnerModule,
  TranslateModule,
  CardModule,
  InputTextModule,
  DropdownModule,
  PaginatorModule,
  SidebarModule,
  ButtonModule,
  CalendarModule,
  FieldsetModule,
  InputMaskModule,
  TableModule,
  DialogModule,
  SkeletonModule,
  MultiSelectModule,
  PasswordModule,
  InputSwitchModule,
  MenuModule,
  MegaMenuModule,
  CheckboxModule,
  TooltipModule,
  AccordionModule,
  TimelineModule,
  FileUploadModule
];

@NgModule({
  declarations: [HasPermissionDirective,ConfirmationComponent],
  imports: [...imports],
  exports: [...imports, HasPermissionDirective,ConfirmationComponent],
})
export class SharedModule { }
