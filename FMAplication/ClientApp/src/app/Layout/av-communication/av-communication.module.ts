import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AvCommunicationListComponent } from './av-communication-list/av-communication-list.component';
import { AddAvCommunicationComponent } from './add-av-communication/add-av-communication.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { AvCommunicationRoutingModule } from './av-communication-routing.module';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { PaginatorModule } from 'primeng/paginator';
import { AvSetupComponent } from './av-setup/av-setup.component';
import { NewAvSetupComponent } from './new-av-setup/new-av-setup.component';
import { EditAvSetupComponent } from './edit-av-setup/edit-av-setup.component';
import { CommunicationSetupComponent } from './communication-setup/communication-setup.component';
import { AddCommunicationSetupComponent } from './add-communication-setup/add-communication-setup.component';
import { EditCommunicationSetupComponent } from './edit-communication-setup/edit-communication-setup.component';



@NgModule({
  declarations: [AvCommunicationListComponent, AddAvCommunicationComponent, 
    AvSetupComponent, NewAvSetupComponent, EditAvSetupComponent, 
    CommunicationSetupComponent, AddCommunicationSetupComponent, EditCommunicationSetupComponent
  ],
  imports: [
    CommonModule, 
    SharedMasterModule,
    ReactiveFormsModule,
    FormsModule,
    AvCommunicationRoutingModule,

    AngularFontAwesomeModule,
    NgbModule,
    NgSelectModule,
    PaginatorModule,

  ]
})
export class AvCommunicationModule { }
