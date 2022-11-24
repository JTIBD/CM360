import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DailyPosmRoutingModule } from './daily-posm-routing.module';
import { DailyPosmAddComponent } from './daily-posm-add/daily-posm-add.component';
import { DailyPosmListComponent } from './daily-posm-list/daily-posm-list.component';
import { SharedMasterModule } from '../../Shared/Modules/shared-master/shared-master.module';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
    declarations: [DailyPosmAddComponent, DailyPosmListComponent],
    imports: [
        CommonModule,
        DailyPosmRoutingModule,

        CommonModule,
        SharedMasterModule,

        AngularFontAwesomeModule,
        NgbModule,
        FormsModule,
        ReactiveFormsModule
    ]
})
export class DailyPosmModule { }
