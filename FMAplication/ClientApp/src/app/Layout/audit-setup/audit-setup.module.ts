import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuditSetupComponent } from './audit-setup/audit-setup.component';
import { AuditSetupRoutingModule } from './audit-setup-routing-module';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { PaginatorModule } from 'primeng/paginator';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NewAuditSetupComponent } from './new-audit-setup/new-audit-setup.component';
import { EditAuditSetupComponent } from './edit-audit-setup/edit-audit-setup.component';



@NgModule({
  declarations: [AuditSetupComponent, NewAuditSetupComponent, EditAuditSetupComponent],
  imports: [
    CommonModule,
    SharedMasterModule,
    ReactiveFormsModule,
    FormsModule,
    // Angular Bootstrap Components
    AngularFontAwesomeModule,
    NgbModule,
    NgSelectModule,
    PaginatorModule,  
    AuditSetupRoutingModule,  

  ]
})
export class AuditSetupModule { }
