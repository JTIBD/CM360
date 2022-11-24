import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RoutesHHTTaskReport } from 'src/app/Shared/Routes/RoutesHHTTaskReport';
import { AuditReportListComponent } from './audit-report-list/audit-report-list.component';
import { AvReportComponent } from './av-report/av-report.component';
import { CommunicationReportComponent } from './communication-report/communication-report.component';
import { ConsumerSurveyReportComponent } from './consumer-survey-report/consumer-survey-report.component';
import { CustomerSurveyReportComponent } from './customer-survey-report/customer-survey-report.component';
import { InformationReportComponent } from './information-report/information-report.component';
import { PosmReportComponent } from './posm-report/posm-report.component';


const routes: Routes = [
  {
    path: '',
    children: [
        { path: '', redirectTo: 'common' },
        { path: RoutesHHTTaskReport.AuditReport, component: AuditReportListComponent },
        { path: RoutesHHTTaskReport.AVReport, component: AvReportComponent },
        { path: RoutesHHTTaskReport.CommunicationReport, component: CommunicationReportComponent },
        { path: RoutesHHTTaskReport.CustomerSurveyReport, component: CustomerSurveyReportComponent },
        { path: RoutesHHTTaskReport.ConsumerSurveyReport, component: ConsumerSurveyReportComponent },
        { path: RoutesHHTTaskReport.InformationReport, component: InformationReportComponent },
        { path: RoutesHHTTaskReport.POSMReport, component: PosmReportComponent },
    ]
}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HhtTaskReportRoutingModule { }
