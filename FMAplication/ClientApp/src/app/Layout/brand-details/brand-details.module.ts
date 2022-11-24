import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrandDetailsRoutingModule } from './brand-details-routing.module';
import { BrandListComponent } from './brand-list/brand-list.component';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { BrandAddComponent } from './brand-add/brand-add.component';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgSelectModule } from '@ng-select/ng-select';
//import { NgOptionHighlightModule } from '@ng-select/ng-option-highlight';



@NgModule({
    declarations: [BrandListComponent, BrandAddComponent],
    imports: [
        CommonModule,
        SharedMasterModule,
        BrandDetailsRoutingModule,
        AngularFontAwesomeModule,
        NgbModule,
        FormsModule,
        ReactiveFormsModule,
        NgSelectModule
        //NgOptionHighlightModule,
    ]
})
export class BrandDetailsModule { }
