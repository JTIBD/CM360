import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HhtTaskReportRoutingModule } from './hht-task-report-routing.module';
import { AuditReportListComponent } from './audit-report-list/audit-report-list.component';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { PaginatorModule } from 'primeng/paginator';
import { AvReportComponent } from './av-report/av-report.component';
import { CommunicationReportComponent } from './communication-report/communication-report.component';
import { CustomerSurveyReportComponent } from './customer-survey-report/customer-survey-report.component';
import { ConsumerSurveyReportComponent } from './consumer-survey-report/consumer-survey-report.component';
import { InformationReportComponent } from './information-report/information-report.component';
import { PosmReportComponent } from './posm-report/posm-report.component';


@NgModule({
  declarations: [AuditReportListComponent, AvReportComponent, CommunicationReportComponent, CustomerSurveyReportComponent, ConsumerSurveyReportComponent, InformationReportComponent, PosmReportComponent],
  imports: [
    CommonModule,
    HhtTaskReportRoutingModule,
    SharedMasterModule,
    PaginatorModule
  ]
})
export class HhtTaskReportModule { }
