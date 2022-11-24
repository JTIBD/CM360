import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { QuestionBankComponent } from './question-bank/question-bank.component';
import { SaveQuestionComponent } from './save-question/save-question.component';
import { QuestionSetGenerationComponent } from './question-set-generation/question-set-generation.component';
import { SurveyListComponent } from './question-sets/survey-list.component';
import { QuestionOptionsComponent } from './question-options/question-options.component';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';
import { SurveySetupComponent } from './survey-setup/survey-setup.component';
import { NewSurveyComponent } from './new-survey/new-survey.component';
import { EditSurveyComponent } from './edit-survey/edit-survey.component';
import { RoutesLaout } from 'src/app/Shared/Routes/RoutesLaout';
import { RoutesQuestionDetails } from 'src/app/Shared/Routes/RoutesQuestionDetails';

const routes: Routes = [
  {
    path: '',
    children: [
      //{path:'', redirectTo:'question-bank'},
      { path:RoutesQuestionDetails.questionSetGeneration, component: QuestionSetGenerationComponent, canActivate: [PermissionGuard], data: { permissionType: 'create', permissionGroup: `${RoutesLaout.Question}/${RoutesQuestionDetails.questionSets}` }/* , data: {extraParameter: 'question'} */},
      { path:`${RoutesQuestionDetails.questionSetGeneration}/:id`, component: QuestionSetGenerationComponent, canActivate: [PermissionGuard], data: { permissionType: 'update', permissionGroup: `${RoutesLaout.Question}/${RoutesQuestionDetails.questionSets}` } },
      { path:RoutesQuestionDetails.questionSets, component: SurveyListComponent, canActivate: [PermissionGuard], data: { permissionType: 'view', permissionGroup: `${RoutesLaout.Question}/${RoutesQuestionDetails.questionSets}` }/* , data: {extraParameter: 'question'} */},
      { path: RoutesQuestionDetails.questionBank, component: QuestionBankComponent, canActivate: [PermissionGuard], data: {extraParameter: 'question', permissionType: 'view', permissionGroup: `${RoutesLaout.Question}/${RoutesQuestionDetails.questionBank}`} },
      { path:RoutesQuestionDetails.saveQuestion, component: SaveQuestionComponent, canActivate: [PermissionGuard], data: { permissionType: 'create', permissionGroup:  `${RoutesLaout.Question}/${RoutesQuestionDetails.questionBank}`}/* , data: {extraParameter: 'question'} */},
      { path: `${RoutesQuestionDetails.saveQuestion}/:id`, component: SaveQuestionComponent, canActivate: [PermissionGuard], data: { permissionType: 'update', permissionGroup: `${RoutesLaout.Question}/${RoutesQuestionDetails.questionBank}` } },
      { path: `${RoutesQuestionDetails.questionOptions}/:questionId`, component: QuestionOptionsComponent },
      { path: RoutesQuestionDetails.surveySetup, component: SurveySetupComponent, canActivate: [PermissionGuard], data: { permissionType: 'view', permissionGroup: `${RoutesLaout.Question}/${RoutesQuestionDetails.surveySetup}` } },
      { path: `${RoutesQuestionDetails.surveySetup}/new`, component: NewSurveyComponent, canActivate: [PermissionGuard], data: { permissionType: 'create', permissionGroup: `${RoutesLaout.Question}/${RoutesQuestionDetails.surveySetup}` } },
      { path: `${RoutesQuestionDetails.surveySetup}/edit/:id`, component: EditSurveyComponent, canActivate: [PermissionGuard], data: { permissionType: 'update', permissionGroup: `${RoutesLaout.Question}/${RoutesQuestionDetails.surveySetup}` } },

    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class QuestionDetailsRoutingModule { }
