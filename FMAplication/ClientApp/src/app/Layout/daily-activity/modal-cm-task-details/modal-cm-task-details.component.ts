import { Component, OnInit, Input } from '@angular/core';
import { DailyCMActivity, PosmProduct, Product, SurveyQuestionSet } from 'src/app/Shared/Entity';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-modal-cm-task-details',
  templateUrl: './modal-cm-task-details.component.html',
  styleUrls: ['./modal-cm-task-details.component.css']
})
export class ModalCmTaskDetailsComponent implements OnInit {

  @Input() dailyCmActivity: DailyCMActivity;
  @Input()  posmInstallationProducts:PosmProduct[]=[];
  @Input()  posmRepairProducts:PosmProduct[]=[];
  @Input()  posmRemovalProducts:PosmProduct[]=[];
  @Input()  distributionCheckProducts:Product[]=[];
  @Input()  facingCountProducts:Product[]=[];
  @Input()  planogramCheckProducts:PosmProduct[]=[];
  @Input()  priceAuditProducts:Product[]=[];
  @Input()  surveys:SurveyQuestionSet[]=[];
  @Input()  consumerSurveys:SurveyQuestionSet[]=[];

  constructor(public activeModal: NgbActiveModal) {
   }

  ngOnInit() {
  }

}
