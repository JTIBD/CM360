import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { QuestionDetailsRoutingModule } from './question-details-routing.module';
import { QuestionBankComponent } from './question-bank/question-bank.component';
import { SaveQuestionComponent } from './save-question/save-question.component';
import { QuestionSetGenerationComponent } from './question-set-generation/question-set-generation.component';
import { SurveyListComponent } from './question-sets/survey-list.component';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

// BOOTSTRAP COMPONENTS
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { QuestionOptionsComponent } from './question-options/question-options.component';
import { SurveySetupComponent } from './survey-setup/survey-setup.component';
import { NewSurveyComponent } from './new-survey/new-survey.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { PaginatorModule } from 'primeng/paginator';
import { EditSurveyComponent } from './edit-survey/edit-survey.component';

@NgModule({
  declarations: [
    QuestionBankComponent,
    SaveQuestionComponent,
    QuestionSetGenerationComponent,
    SurveyListComponent,
    QuestionOptionsComponent,
    SurveySetupComponent,
    NewSurveyComponent,
    EditSurveyComponent,
  ],
  imports: [
    CommonModule,
    SharedMasterModule,
    ReactiveFormsModule,
    FormsModule,
    QuestionDetailsRoutingModule,
    // Angular Bootstrap Components
    AngularFontAwesomeModule,
    NgbModule,
    NgSelectModule,
    PaginatorModule,

  ]
})
export class QuestionDetailsModule { }
