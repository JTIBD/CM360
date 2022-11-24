import { ExecutionLimitAddComponent } from './execution-limit-add/execution-limit-add.component';
import { ExecutionLimitListComponent } from './execution-limit-list/execution-limit-list.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ExecutionLimitRoutingModule } from './execution-limit-routing.module';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { PaginatorModule } from 'primeng/paginator';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ExecutionLimitEditComponent } from './execution-limit-edit/execution-limit-edit.component';


@NgModule({
  declarations: [ExecutionLimitListComponent, ExecutionLimitAddComponent, ExecutionLimitEditComponent],
  imports: [
    CommonModule,
    ExecutionLimitRoutingModule,
    SharedMasterModule,
    ReactiveFormsModule,
    FormsModule,
    NgSelectModule,
    PaginatorModule,
    NgbModule
  ]
})
export class ExecutionLimitModule { }
