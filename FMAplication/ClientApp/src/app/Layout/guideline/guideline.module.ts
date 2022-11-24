import { SharedMasterModule } from './../../Shared/Modules/shared-master/shared-master.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GuidelineRoutingModule } from './guideline-routing.module';
import { GuidelineSetupComponent } from './guideline-setup/guideline-setup.component';
import { NewGuidelineSetupComponent } from './new-guideline-setup/new-guideline-setup.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { PaginatorModule } from 'primeng/paginator';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { EditGuidelineSetupComponent } from './edit-guideline-setup/edit-guideline-setup.component';

@NgModule({
  declarations: [GuidelineSetupComponent, NewGuidelineSetupComponent, EditGuidelineSetupComponent],
  imports: [
    CommonModule,
    GuidelineRoutingModule,
    SharedMasterModule,
    ReactiveFormsModule,
    FormsModule,
    NgbModule,
    NgSelectModule,
    PaginatorModule,
    AngularEditorModule
  ]
})
export class GuidelineModule { }
