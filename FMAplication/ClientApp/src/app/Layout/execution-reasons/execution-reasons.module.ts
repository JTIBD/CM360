import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ExecutionReasonsRoutingModule } from './execution-reasons-routing.module';
import { ExecutionReasonListComponent } from './execution-reason-list/execution-reason-list.component';
import { ExecutionReasonAddComponent } from './execution-reason-add/execution-reason-add.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgSelectModule } from '@ng-select/ng-select';

@NgModule({
  declarations: [ExecutionReasonListComponent, ExecutionReasonAddComponent],
  imports: [
    CommonModule,
    ExecutionReasonsRoutingModule,
    CommonModule,
    SharedMasterModule,
    AngularFontAwesomeModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule
  ]
})
export class ExecutionReasonsModule { }
