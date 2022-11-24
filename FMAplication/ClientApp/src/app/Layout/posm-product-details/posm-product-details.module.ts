import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PosmProductDetailsRoutingModule } from './posm-product-details-routing.module';
import { PosmProductListComponent } from './posm-product-list/posm-product-list.component';
import { PosmProductAddComponent } from './posm-product-add/posm-product-add.component';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgSelectModule } from '@ng-select/ng-select';
//import { NgOptionHighlightModule } from '@ng-select/ng-option-highlight';



@NgModule({
    declarations: [PosmProductListComponent, PosmProductAddComponent],
    imports: [
        CommonModule,
        SharedMasterModule,
        PosmProductDetailsRoutingModule,
        AngularFontAwesomeModule,
        NgbModule,
        FormsModule,
        ReactiveFormsModule,
        NgSelectModule
        //NgOptionHighlightModule,
    ]
})
export class PosmProductDetailsModule { }
