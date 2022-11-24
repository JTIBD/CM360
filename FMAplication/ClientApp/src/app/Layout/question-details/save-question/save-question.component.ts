import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormControl } from '@angular/forms';
import { Question } from '../../../Shared/Entity/Questions/question';
import { QuestionService } from 'src/app/Shared/Services/Question-Details/question.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Status } from 'src/app/Shared/Enums/status';
import { QuestionTypes } from 'src/app/Shared/Enums/questionTypes';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { RoutesQuestionDetails } from 'src/app/Shared/Routes/RoutesQuestionDetails';


@Component({
  selector: 'app-save-question',
  templateUrl: './save-question.component.html',
  styleUrls: ['./save-question.component.sass']
})
export class SaveQuestionComponent implements OnInit {

  public form: FormGroup;
  question: Question = new Question();

  questionTypes: {};
  questionTypeOptions: {};
  statusOptions: {};
  keys: string[];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private questionService: QuestionService,
    private alertService: AlertService
  ) { }

  ngOnInit() {
    this.createForm();
    // console.log("param", this.route.snapshot.params, Object.keys(this.route.snapshot.params).length);

    if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
      // console.log("id", this.route.snapshot.params.id);
      let questionId = this.route.snapshot.params.id;
      this.getQuestion(questionId);
    }
  }

  createForm() {
    this.questionTypes = QuestionTypes;
    this.questionTypeOptions = Object.keys(this.questionTypes);
    // console.log("questionTypes: ", this.questionTypes, this.questionTypeOptions);

    this.statusOptions = Status;
    this.keys = Object.keys(this.statusOptions).filter(k => !isNaN(Number(k)));
    // console.log("statusOptions: " , this.statusOptions, this.keys);

    this.form = this.fb.group({
      questionTitle: ['', [Validators.required, Validators.minLength(3)]],
      questionType: [this.questionTypeOptions[0]],
      status: Status.Active
    });
  }

  getQuestion(questionId) {
    this.questionService.getQuestionById(questionId).subscribe(
      (result: any) => {
        console.log("questions data", result.data);
        this.editForm(result.data);
      },
      (err: any) => console.log(err)
    );
  };

  editForm(question: any) {
    debugger;
    this.question.id = question.id;
    this.question.questionType = question.questionType;
    this.question.questionOptions = question.questionOptions;

    this.form.patchValue({
      //id: question.id,
      questionTitle: question.questionTitle,
      questionType: question.questionType,
      status: question.status
    });

    let index = 0;

    this.question.questionOptions.forEach( q => {

      q.serialNumber = index++;
      

    });

  }

  bindQueObject(event) {
    this.question.questionType = this.questionTypeControl.value;
    // if(this.question.questionType == QuestionTypes.Rating || this.question.questionType == QuestionTypes.Emo){
    //   this.question.questionOptions = [];
    // }
    this.question.questionOptions = [];
    console.log("que: ", this.question);
  }

  createNewQueOption(question) {
    // console.log("Question change track: ", question);
    this.question.questionOptions = question.questionOptions;
  }

  get questionTitleControl(): FormControl {
    return this.form.get('questionTitle') as FormControl;
  }

  get questionTypeControl(): FormControl {
    return this.form.get('questionType') as FormControl;
  }

  get statusControl(): FormControl {
    return this.form.get('status') as FormControl;
  }

  submit(): void {
    
    this.question.questionTitle = this.questionTitleControl.value;
    this.question.questionType = this.questionTypeControl.value;
    this.question.status = this.statusControl.value;
    this.question.questionOptions.forEach(q =>{

      if(q.id < 0)
      {
        q.id = 0;
      }


    });
    // console.log(this.question);
    this.question.id == 0 ? this.insertQuestion() : this.updateQuestion();
  }

  validateQuestionOptions(){
    const questionTypesHavingOptions:string[] = [QuestionTypes.SingleChoice,QuestionTypes.MultipleChoice,QuestionTypes.Dropdown,QuestionTypes.Emo];
    const selectedOptionValue = QuestionTypes[this.question.questionType];
    if(questionTypesHavingOptions.includes(selectedOptionValue) ){
      if(!this.question.questionOptions || !this.question.questionOptions.length){
        this.alertService.tosterDanger("No options provided.");
        throw "invalid option";
      }
    }
    if(selectedOptionValue === QuestionTypes.Slider){
      var checkValueExists = this.question.questionOptions.map(x => !!x.optionTitle).includes(false);
      if(checkValueExists || !this.question.questionOptions || this.question.questionOptions.length !== 2 ){
        this.alertService.tosterDanger("maximum and minimum value required for slider question.");
        throw "invalid option";
      }
    }

    if(selectedOptionValue === QuestionTypes.Rating){      
      if(!this.question.questionOptions || this.question.questionOptions.length !== 1 || 
        this.question.questionOptions.map(x => !!x.optionTitle).includes(false)){
        this.alertService.tosterDanger("maximum value required for rating question.");
        throw "invalid option";
      }
    }
  }

  insertQuestion() {
    console.log('validating');
    this.validateQuestionOptions();
    console.log('validated');

    this.questionService.insertQuestion(this.question).subscribe(res => {
      this.router.navigate(['/question/question-bank']).then(() => {
        this.alertService.tosterSuccess("Record has been saved successfully.");
      });
    },
      (error) => {
        console.log(error);
        this.displayError(error);
      }, () => this.alertService.fnLoading(false)
    );
  }

  updateQuestion() {
    
    console.log('validating',this.question);
    this.validateQuestionOptions();
    console.log('validated');
    this.questionService.updateQuestion(this.question).subscribe(res => {
      this.router.navigate(['/question/question-bank']).then(() => {
        this.alertService.tosterSuccess("Record has been updated successfully.");
      });
    },
      (error) => {
        console.log(error);
        this.displayError(error.message);
      }, () => this.alertService.fnLoading(false)
    );
  }

  handleBack(){
    this.router.navigate([RoutesQuestionDetails.parent,RoutesQuestionDetails.questionBank]);
  }

  displayError(errorDetails: any) {
    // this.alertService.fnLoading(false);
    console.log("error", errorDetails);
    let errList = errorDetails.error.errors;
    if (errList.length) {
      console.log("error", errList, errList[0].errorList[0]);
      this.alertService.tosterDanger(errList[0].errorList[0]);
    } else {
      this.alertService.tosterDanger(errorDetails.error.msg);
    }
  }

}
