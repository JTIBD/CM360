import { Component, OnInit, Input, Output, EventEmitter, SimpleChanges, ViewChild, TemplateRef } from '@angular/core';
import { QuestionOption } from 'src/app/Shared/Entity/Questions/questionOption';
import { Router, ActivatedRoute } from '@angular/router';
import { QuestionService } from 'src/app/Shared/Services/Question-Details/question.service';
import { Question } from 'src/app/Shared/Entity/Questions/question';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Status } from 'src/app/Shared/Enums/status';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { QuestionTypes } from 'src/app/Shared/Enums/questionTypes';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Emoticons } from 'src/app/Shared/Enums/Emoticons';

@Component({
  selector: 'app-question-options',
  templateUrl: './question-options.component.html',
  styleUrls: ['./question-options.component.css']
})
export class QuestionOptionsComponent implements OnInit {
  @Input() question: Question;
  @Output() createNewOption = new EventEmitter<Question>();

  public formQueOption: FormGroup;

  questionOption: QuestionOption[] = [];
  closeResult: string;
  selectedOptionId: number = 0;
  statusOptions: {};
  keys: string[];
  questionTypes = QuestionTypes;
  debounce: number = 400;
  emoOptions: {};
  emoKeys: string[];
  emoChecked: {} = {};
  masterIndex: number;
  @ViewChild('content', { static: false }) content: any;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private questionService: QuestionService,
    private modalService: NgbModal,
    private fb: FormBuilder,
    private alertService: AlertService
  ) { }

  ngOnInit() {
    console.log("question options: ", this.question);
    this.questionTypes = QuestionTypes;
    this.masterIndex = -1;
    // console.log("question types: ", this.questionTypes, this.questionTypes[this.question.questionType]);
    this.createForm();

    if (this.question.questionOptions.length) {
      this.emoKeys.forEach(key => {
        let selectedOption = this.question.questionOptions.find(o => o.optionTitle == key);
        this.emoChecked[key] = selectedOption != null ? "checked" : "";
      });
    }

    this.question.questionOptions.forEach(obj => {
      obj.statusText = this.statusOptions[obj.status];
    });

  }

  ngDoCheck() {
    //  console.log("this.question: ", this.question);

    if (this.questionTypes[this.question.questionType] == QuestionTypes.Emo && this.question.questionOptions.length) {
      this.emoKeys.forEach(key => {
        let selectedOption = this.question.questionOptions.find(o => o.optionTitle == key);
        this.emoChecked[key] = selectedOption != null ? "checked" : "";
      });
    }
    if (this.questionTypes[this.question.questionType] == QuestionTypes.SingleChoice ||
      this.questionTypes[this.question.questionType] == QuestionTypes.MultipleChoice ||
      this.questionTypes[this.question.questionType] == QuestionTypes.Dropdown) {
      this.question.questionOptions.forEach(obj => {
        obj.statusText = this.statusOptions[obj.status];
      });
      // console.log("this.question: ", this.question);
    }

    if (this.question.questionType == QuestionTypes.Slider) {
      this.formQueOption.controls['maxValue'].setValidators([Validators.required]);
      this.formQueOption.controls['minValue'].setValidators([Validators.required]);
    }


    // if (this.question.questionType == QuestionTypes.Rating) {
    //   this.optionTitleControl.valueChanges
    //     .pipe(debounceTime(this.debounce), distinctUntilChanged())
    //     .subscribe(query => {
    //       console.log(query);
    //       this.addToOption();
    //     });
    // }
  }

  createForm() {
    console.log(this.question.questionOptions);
    this.statusOptions = Status;
    this.keys = Object.keys(this.statusOptions).filter(k => !isNaN(Number(k)));

    this.emoOptions = Emoticons;
    this.emoKeys = Object.keys(this.emoOptions);

    //console.log("this.emoOptions", this.emoOptions, Object.keys(this.emoOptions));
    this.formQueOption = this.fb.group({
      optionTitle: ['', Validators.required],
      maxValue: [''],
      minValue: [''],
      sequence: 0,
      status: Status.Active
    });
  }

  createNew(): void {
    let queOption: QuestionOption = new QuestionOption();

    console.log(this.question.questionOptions);

    if (this.selectedOptionId != 0) {
      let objIndex = this.question.questionOptions.findIndex((obj => obj.id == this.selectedOptionId));
      // console.log("Before update: ", this.question.questionOptions[objIndex])
      queOption = this.question.questionOptions[objIndex];
    }
    //  else if(this.selectedOptionId == 0)
    //   {
    //     let objIndex = this.question.questionOptions.findIndex((obj => obj.sequence == this.sequenceControl.value));
    //     // console.log("Before update: ", this.question.questionOptions[objIndex])

    //     if(objIndex != -1)
    //     {
    //       queOption = this.question.questionOptions[objIndex];
    //     }

    //   }

    queOption.optionTitle = this.optionTitleControl.value;
    queOption.sequence = this.sequenceControl.value;
    queOption.status = this.statusControl.value;


    if (this.selectedOptionId == 0) {
      queOption.id = this.masterIndex--;
      this.question.questionOptions.push(queOption);
    }

    this.question.questionOptions.forEach(obj => {
      obj.statusText = this.statusOptions[obj.status];
    });

    this.question.questionOptions.sort((a, b) => (a.sequence > b.sequence) ? 1 : ((b.sequence > a.sequence) ? -1 : 0));
    // console.log("question: ", this.question);
    this.createNewOption.emit(this.question);
  }

  createNewQuestion() {
    this.router.navigate(['/question/save-question']);
  }

  open(content, selectedOption: QuestionOption = null, serialNo: number = -1) {

    if (selectedOption != null) {
      this.selectedOptionId = selectedOption.id;
      // let selectedOption = this.question.questionOptions.find(o=>o.id == id);
      this.formQueOption.patchValue({
        //id: selectedOption.id,
        optionTitle: selectedOption.optionTitle,
        sequence: selectedOption.sequence,
        status: selectedOption.status
      });
    } else {
      this.selectedOptionId = 0;
      this.resetFormValue();
    }

    this.modalService.open(content).result.then((result) => {
      if (result == "add-more-click" || result == "add-close-click") {
        this.createNew();
        if (result == "add-more-click") {
          this.open(content, selectedOption);
        }
      }
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }

  addToOption() {
    this.selectedOptionId = this.question.questionOptions.length != 0 ? this.question.questionOptions[0].id : 0;
    this.createNew();
  }
  addMaxToOption() {
    this.selectedOptionId = this.question.questionOptions.length > 1 ? this.question.questionOptions[1].id : 0;
    let queOption: QuestionOption = new QuestionOption();

    console.log(this.question.questionOptions);

    if (this.selectedOptionId != 0) {
      let objIndex = this.question.questionOptions.findIndex((obj => obj.id == this.selectedOptionId));
      // console.log("Before update: ", this.question.questionOptions[objIndex])
      queOption = this.question.questionOptions[objIndex];
    }

    queOption.optionTitle = this.titleControl('maxValue').value;
    queOption.sequence = this.sequenceControl.value;
    queOption.status = this.statusControl.value;

    if (this.selectedOptionId == 0) {
      queOption.id = this.masterIndex--;
      this.question.questionOptions.push(queOption);
    }

    this.question.questionOptions.sort((a, b) => (a.sequence > b.sequence) ? 1 : ((b.sequence > a.sequence) ? -1 : 0));
    // console.log("question: ", this.question);
    this.createNewOption.emit(this.question);
  }

  addMinToOption() {
    this.selectedOptionId = this.question.questionOptions.length != 0 ? this.question.questionOptions[0].id : 0;
    let queOption: QuestionOption = new QuestionOption();

    console.log(this.question.questionOptions);

    if (this.selectedOptionId != 0) {
      let objIndex = this.question.questionOptions.findIndex((obj => obj.id == this.selectedOptionId));
      // console.log("Before update: ", this.question.questionOptions[objIndex])
      queOption = this.question.questionOptions[objIndex];
    }

    queOption.optionTitle = this.titleControl('minValue').value;
    queOption.sequence = this.sequenceControl.value;
    queOption.status = this.statusControl.value;

    if (this.selectedOptionId == 0) {
      queOption.id = this.masterIndex--;
      this.question.questionOptions.push(queOption);
    }

    this.question.questionOptions.sort((a, b) => (a.sequence > b.sequence) ? 1 : ((b.sequence > a.sequence) ? -1 : 0));
    // console.log("question: ", this.question);
    this.createNewOption.emit(this.question);
  }

  addEmoOption(event, key) {
    console.log("checkbox: ", event);


    let selectedOption = this.question.questionOptions.find(o => o.optionTitle == key);

    if (selectedOption != null && event.target.checked == false) {
      this.question.questionOptions = this.question.questionOptions.filter((elm) => {
        return elm.optionTitle != key;
      });
      console.log("updated questionOptions: ", this.question.questionOptions);

      return;
    }

    this.selectedOptionId = selectedOption != null ? selectedOption.id : 0;
    let queOption: QuestionOption = new QuestionOption();

    if (this.selectedOptionId != 0) {
      let objIndex = this.question.questionOptions.findIndex((obj => obj.id == this.selectedOptionId));
      // console.log("Before update: ", this.question.questionOptions[objIndex])
      queOption = this.question.questionOptions[objIndex];
    }

    queOption.optionTitle = key;
    // queOption.sequence = 0;
    queOption.status = Status.Active;
    queOption.sequence = this.emoKeys.findIndex(x=>x == queOption.optionTitle);

    if (this.selectedOptionId == 0) {
      this.question.questionOptions.push(queOption);
    }

    console.log("question: ", this.question);
    this.createNewOption.emit(this.question);
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  get optionTitleControl(): FormControl {
    return this.formQueOption.get('optionTitle') as FormControl;
  }
  titleControl(title = 'optionTitle'): FormControl {
    return this.formQueOption.get(title) as FormControl;
  }

  get sequenceControl(): FormControl {
    return this.formQueOption.get('sequence') as FormControl;
  }

  get statusControl(): FormControl {
    return this.formQueOption.get('status') as FormControl;
  }

  backToMenuList() {
    this.router.navigate(['/menu/menu-list']);
  }

  resetFormValue() {
    let seq = this.question.questionOptions.length == 0 ? 1 : this.question.questionOptions.length + 1;
    // this.formQueOption.patchValue({
    //   //id: selectedOption.id,
    //   optionTitle: "",
    //   sequence: seq,
    //   status: Status.Active
    // });

    this.formQueOption = this.fb.group({
      optionTitle: ['', Validators.required],
      sequence: seq,
      status: Status.Active
    });
  }

  delete(index: number) {
    this.alertService.confirm("Are you sure you want to delete this item?", () => {
      // this.question.questionOptions = this.question.questionOptions.filter((optionObj) => {
      //   return optionObj.id != id;
      // });
      let removeIndex = this.question.questionOptions.findIndex(qo => qo.id == index);
      this.question.questionOptions.splice(removeIndex, 1);
    }, () => {

    });
  }


  public ptableSettings = {
    tableID: "questionOption-table",
    tableClass: "table-responsive",

    tableRowIDInternalName: "Id",
    tableColDef: [

      { headerName: 'Option Title', width: '50%', internalName: 'optionTitle', sort: true, type: "" },
      { headerName: 'Sequence', width: '30%', internalName: 'sequence', sort: true, type: "" },
      { headerName: 'Status', width: '20%', internalName: 'statusText', sort: true, type: "" }

    ],
    enabledSearch: false,
    enabledSerialNo: true,
    pageSize: 10,
    enabledPagination: false,
    enabledEditDeleteBtn: true,
    enabledColumnFilter: true,
    enabledRecordCreateBtn: true,
  };


  public fnCustomTrigger(event) {
    console.log("custom  click: ", event);

    if (event.action == "new-record") {
      this.open(this.content);
    }
    else if (event.action == "edit-item") {
      this.open(this.content, event.record);
    }
    else if (event.action == "delete-item") {
      this.delete(event.record.id);
    }
  }

}
