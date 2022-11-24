import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DailyAuditRoutingModule } from './daily-audit-routing.module';
import { DailyAuditAddComponent } from './daily-audit-add/daily-audit-add.component';
import { DailyAuditListComponent } from './daily-audit-list/daily-audit-list.component';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [DailyAuditAddComponent, DailyAuditListComponent],
  imports: [
    CommonModule,
    DailyAuditRoutingModule,
        SharedMasterModule,

        AngularFontAwesomeModule,
        NgbModule,
        FormsModule,
        ReactiveFormsModule
  ]
})
export class DailyAuditModule { }
