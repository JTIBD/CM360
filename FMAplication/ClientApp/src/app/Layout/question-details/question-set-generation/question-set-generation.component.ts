import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { SurveyQuestionSet } from 'src/app/Shared/Entity/Questions/survey';
import { QuestionService } from 'src/app/Shared/Services/Question-Details/question.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Question } from 'src/app/Shared/Entity/Questions/question';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { Status } from 'src/app/Shared/Enums/status';
import { SurveyService } from 'src/app/Shared/Services/Question-Details/survey.service';
import { QuestionTypes } from 'src/app/Shared/Enums/questionTypes';
import { RoutesLaout } from 'src/app/Shared/Routes/RoutesLaout';
import { RoutesQuestionDetails } from 'src/app/Shared/Routes/RoutesQuestionDetails';

@Component({
	selector: 'app-question-set-generation',
	templateUrl: './question-set-generation.component.html',
	styleUrls: ['./question-set-generation.component.sass']
})
export class QuestionSetGenerationComponent implements OnInit {
	public form: FormGroup;
	public questions: Question[] = [];
	public selectedQuestions: Question[] = [];
	public survey: SurveyQuestionSet;
	public enumStatus = Status;
	public enumQuestionType = QuestionTypes;
	public statusValues = [];
	public questionTypes = [];
	public toasterMsg: string = "Record has been saved successfully.";

	constructor(
		private questionService: QuestionService,
		private surveyService: SurveyService,
		private router: Router,
		private activatedRoute: ActivatedRoute,
		private alertService: AlertService
	) { }

	ngOnInit() {
		this.survey = new SurveyQuestionSet();
		this.statusValues = Object.keys(this.enumStatus).filter(e => !isNaN(Number(e)));
		this.getQuestions();
	}

	getQuestions() {
		this.alertService.fnLoading(true);
		this.questionService.getActiveQuestions().subscribe(
			(res: any) => {
				console.log(res.data);
				this.questions = res.data || [];
				this.questions.forEach(q => {
					q.addToSurveyBtn = 'Add To Survey';
					q.questionType = this.enumQuestionType[q.questionType];
				});
				if ((this.activatedRoute.snapshot.params).hasOwnProperty('id')) {
					this.getSurveyById(+this.activatedRoute.snapshot.params.id);
				}
			},
			(error) => {
				console.log(error);
				this.alertService.fnLoading(false);
				this.showError(error.message);
			},
			() => {
				this.alertService.fnLoading(false);
			}
		);
	}

	getSurveyById(id: number) {
		this.alertService.fnLoading(true);
		this.surveyService.getQuestionsetById(id).subscribe(
			(res: any) => {
				this.survey = res.data || new SurveyQuestionSet();

				if (Object.keys(res.data).length == 0) {
					this.showError("No such survey!Create a new instead");
					this.router.navigate([RoutesLaout.Question,RoutesQuestionDetails.questionSets]);
				}
				else {
					this.survey.questionsId.forEach(id => this.addToSelectedBucket(id));
				}
			},
			(error) => {
				this.showError(error.message);
				this.router.navigate([RoutesLaout.Question,RoutesQuestionDetails.questionSets]);
			},
			() => {
				this.alertService.fnLoading(false);
			}
		);
	}

	addToSelectedBucket(questionId: number) {
		if (!this.isPresentInBucket(questionId)) {
			let addedQs = this.questions.find(q => q.id == questionId);
			if(addedQs) this.selectedQuestions.push(addedQs);
			this.questions.splice(this.questions.findIndex(q => q.id == questionId), 1);
		}
	}

	removeFromSelectedBucket(questionId: number) {
		this.alertService.confirm("Are you sure?",
			() => {
				if (this.isPresentInBucket) 
				{
					let removedQs = this.selectedQuestions.find(q => q.id == questionId);
					this.questions.push(removedQs);
					this.selectedQuestions.splice(this.selectedQuestions.findIndex(q => q.id == questionId), 1);
				}
			},
			() => { });
	}

	removeAll() {
		this.alertService.confirm("Are you sure to empty selected bucket?",
			() => {
				if (this.selectedQuestions.length > 0) 
				{
					this.questions = this.questions.concat(this.selectedQuestions);
					this.selectedQuestions = [];
				}
			},
			() => { });
	}

	isPresentInBucket(questionId: number): boolean {
		var isExist = (this.selectedQuestions.find(q => q.id == questionId)) ? true : false;
		return isExist
	}

	submit(): void {
		this.survey.questionsId = this.selectedQuestions.map(sq => sq.id);
		console.log(this.survey);
		if(!this.survey.questionsId || !this.survey.questionsId.length){
			this.alertService.tosterDanger("No question selected.");
			return;
		}
		(this.survey.id > 0) ? this.updateSurvey() : this.createSurvey();
	}

	private createSurvey() {
		this.alertService.fnLoading(true);
		this.surveyService.createSurvey(this.survey).subscribe((res: any) => {
			this.router.navigate([RoutesLaout.Question,RoutesQuestionDetails.questionSets]);
			this.alertService.tosterSuccess(this.toasterMsg);
		}, (error) => {
			console.log(error);
			this.showError(error.error.errors[0].errorList[0]);
		}, () => this.alertService.fnLoading(false));
	}

	private updateSurvey() {
		this.alertService.fnLoading(true);
		this.surveyService.updateSurvey(this.survey).subscribe((res: any) => {
			this.router.navigate([RoutesLaout.Question,RoutesQuestionDetails.questionSets]);
			this.alertService.tosterSuccess(this.toasterMsg);
		}, (error) => {
			this.showError(error.error.errors[0].errorList[0]);
		}, () => this.alertService.fnLoading(false));
	}

	showError(msg: string) {
		this.alertService.fnLoading(false);
		this.alertService.tosterDanger(msg);
		//alert(msg);
	}

	public ptableSettings = {
		tableID: "Question-table",
		tableClass: "table-responsive",
		tableName: 'Active Questions',
		tableRowIDInternalName: "Id",
		tableColDef: [
			{ headerName: 'Name', width: '60%', internalName: 'questionTitle', sort: true, type: "" },
			{ headerName: 'Question Type', width: '20%', internalName: 'questionType', sort: true, type: "" },
			{ headerName: 'Add Question', width: '20%', internalName: 'addToSurveyBtn', sort: true, type: "button" ,onClick: 'true', innerBtnIcon:"fa fa-plus"}
		],
		enabledSearch: true,
		enabledSerialNo: true,
		pageSize: 5,
		enabledPagination: true,
		enabledCellClick: true,
		enabledColumnFilter: true,
		enabledRecordCreateBtn: false,
	};

	handleBack(){
		this.router.navigate([RoutesQuestionDetails.parent,RoutesQuestionDetails.questionSets]);
	  }

	public addToBtnClick(event: any) {
		this.addToSelectedBucket(event.record.id);
	}
}