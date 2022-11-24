import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CampaignDetailsRoutingModule } from './campaign-details-routing.module';
import { CampaignListComponent } from './campaign-list/campaign-list.component';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { CampaignAddComponent } from './campaign-add/campaign-add.component';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgSelectModule } from '@ng-select/ng-select';
import { CampaignDetailsComponent } from './campaign-details/campaign-details.component';
//import { NgOptionHighlightModule } from '@ng-select/ng-option-highlight';



@NgModule({
    declarations: [CampaignListComponent, CampaignAddComponent, CampaignDetailsComponent],
    imports: [
        CommonModule,
        SharedMasterModule,
        CampaignDetailsRoutingModule,
        AngularFontAwesomeModule,
        NgbModule,
        FormsModule,
        ReactiveFormsModule,
        NgSelectModule
        //NgOptionHighlightModule,
    ]
})
export class CampaignDetailsModule { }
