import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SubBrandDetailsRoutingModule } from './subbrand-details-routing.module';
import { SubBrandListComponent } from './subbrand-list/subbrand-list.component';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { SubBrandAddComponent } from './subbrand-add/subbrand-add.component';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgSelectModule } from '@ng-select/ng-select';
//import { NgOptionHighlightModule } from '@ng-select/ng-option-highlight';



@NgModule({
    declarations: [SubBrandListComponent, SubBrandAddComponent],
    imports: [
        CommonModule,
        SharedMasterModule,
        SubBrandDetailsRoutingModule,
        AngularFontAwesomeModule,
        NgbModule,
        FormsModule,
        ReactiveFormsModule,
        NgSelectModule
        //NgOptionHighlightModule,
    ]
})
export class SubBrandDetailsModule { }
