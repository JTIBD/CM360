import { Router } from '@angular/router';
import { GuidelineSetupService } from './../../../Shared/Services/GuidelineSetup/guidelineSetup.service';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import * as moment from 'moment';
import { TaskAssignedUserType, TaskAssignedUserTypeStrs } from 'src/app/Shared/Enums';
import { NodeTree, SalesPoint } from 'src/app/Shared/Entity';
import { UserService } from 'src/app/Shared/Services/Users';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { GuidelineSetup } from 'src/app/Shared/Entity/Guidelines/guideline-setup';
import { IDateRange } from 'src/app/Shared/interfaces';
import { Utility } from 'src/app/Shared/utility';
import { first } from 'rxjs/operators';
import { RoutesGuidelineModule } from 'src/app/Shared/Routes/RoutesGuidelineModule';

@Component({
  selector: 'app-new-guideline-setup',
  templateUrl: './new-guideline-setup.component.html',
  styleUrls: ['./new-guideline-setup.component.css']
})
export class NewGuidelineSetupComponent implements OnInit {

  public selectedFromDate: NgbDateStruct;
  public selectedToDate: NgbDateStruct;
  public minDate: NgbDateStruct;
  selectedUserType: TaskAssignedUserType = TaskAssignedUserType.BOTH;
  userTypes = TaskAssignedUserTypeStrs;
  nodeTree: NodeTree[] = [];

  salesPointIds: number[] = [];

  htmlContent = '';
  config: AngularEditorConfig;

  constructor(private commonService: CommonService, private userService: UserService,
    private alertService: AlertService, private guidelineSetupService:GuidelineSetupService,
    private router: Router) { }

  ngOnInit() {
    this.minDate = this.commonService.dateToNgbDate(new Date());
    this.richTextBoxConfig();

    this.userService.getNodeTreeByCurrentUser().subscribe(data => {
      this.nodeTree = data;
    })
  }

  handleFromDateChange() {
    let from = this.commonService.ngbDateToDate(this.selectedFromDate).toISOString();
    let toDate = this.commonService.ngbDateToDate(this.selectedToDate).toISOString();
    if (moment(from).isAfter(toDate, "date")) {
      this.selectedToDate = null;
    }
  }
  handleToDateChange() {
    let from = this.commonService.ngbDateToDate(this.selectedFromDate).toISOString();
    let toDate = this.commonService.ngbDateToDate(this.selectedToDate).toISOString();
    if (moment(from).isAfter(toDate, "date")) {
      this.selectedFromDate = null;
    }
  }

  submit() {
    const guidelineSetupList:GuidelineSetup[]=[];
    
    const fromDateStr = this.commonService.ngbDateToDate(this.selectedFromDate).toISOString();
    const toDateStr = this.commonService.ngbDateToDate(this.selectedToDate).toISOString();
    const dateRange:IDateRange={
      from:fromDateStr,
      to:toDateStr,
    };
    Utility.adjustDateRange(dateRange);
    const salesPoints = this.getSelectedSalesPoints();
    if(!salesPoints.length) this.alertService.titleTosterDanger("No salespoint selected");
    salesPoints.forEach(sp=>{
      const gs = new GuidelineSetup();
      gs.code="G_"+sp.code;
      gs.fromDate=dateRange.from;
      gs.toDate = dateRange.to;
      gs.salesPointId = sp.salesPointId;
      gs.guidelineText = this.htmlContent;
      gs.userType = this.selectedUserType;
      guidelineSetupList.push(gs);
    });
    
    this.guidelineSetupService.getExistingGuidelineSetup({data:guidelineSetupList}).pipe(first()).subscribe(res=>{
      if(res.length) {
        if(this.alertService.loadingFlag) this.alertService.fnLoading(false);
        let sps = salesPoints.filter(x=> res.some(sv=>sv.salesPointId == x.salesPointId)).filter((sp,i,arr)=> arr.findIndex(x=>x.salesPointId == sp.salesPointId) === i);
        let spNames = sps.map(x=>x.name);

        this.alertService.confirm(`Guideline setup already exist in Salespoint ${spNames.join(", ")}. Do you want to stop the Guideline Setups before the new guideline starts?`, () => {
          this.guidelineSetupService.createNewGuidelineSetup({ data: guidelineSetupList }).subscribe(res => {
            console.log(res);
            this.alertService.tosterSuccess("Guideline setup created successfully");
            this.router.navigate(["/guideline/guideline-setup"]);
          });
        },()=>{}); 

      }
      else{
        this.guidelineSetupService.createNewGuidelineSetup({ data: guidelineSetupList }).subscribe(res => {
          console.log(res);
          this.alertService.tosterSuccess("Guideline setup created successfully");
          this.router.navigate(["/guideline/guideline-setup"]);
        });
      }
    })

  }

  getSelectedSalesPoints(){
    const salesPointIds:SalesPoint[]=[];
    let fun=(tree:NodeTree[])=>{
      tree.forEach(tr=>{
        if(!!tr.salesPoints && !!tr.salesPoints.length){
          tr.salesPoints.forEach(x=>{
            if(x.isSelected) salesPointIds.push(x);
          })
        }
        else if(!!tr.nodes) fun(tr.nodes);
      })
    }
    fun(this.nodeTree);
    return  salesPointIds;
  }

  richTextBoxConfig() {
    this.config = {
      editable: true,
      minHeight: '10rem',
      spellcheck: true,
      translate: 'yes',
      enableToolbar: true,
      showToolbar: true,
      placeholder: 'Enter text here...',
      defaultParagraphSeparator: '',
      defaultFontName: '',
      defaultFontSize: '',
      fonts: [
        { class: 'arial', name: 'Arial' },
        { class: 'times-new-roman', name: 'Times New Roman' },
        { class: 'calibri', name: 'Calibri' },
        { class: 'comic-sans-ms', name: 'Comic Sans MS' }
      ],
      uploadWithCredentials: false,
      sanitize: true,
      toolbarPosition: 'top',
      toolbarHiddenButtons: [
        [
          // 'fontName'
        ],
        [
          // 'fontSize',
          // 'textColor',
          // 'backgroundColor',
          'customClasses',
          'link',
          'unlink',
          'insertImage',
          'insertVideo',
          'insertHorizontalRule',
          'removeFormat',
          'toggleEditorMode'
        ]
      ]
    };
  }

  handleSalesPointSelect(event, salesPoint: SalesPoint) {
    salesPoint.isSelected = event.target.checked;
  }

  handleNodeSelect(item: NodeTree, checked: boolean) {
    console.log(item, checked);
    const node: NodeTree = this.getNodeById(item.node.id);
    console.log(node);
    if (!node) return;
    let fun = (trees: NodeTree[], checked: boolean) => {
      trees.forEach(tr => {
        tr.isSelected = checked;
        if (!!tr.nodes && tr.nodes.length) fun(tr.nodes, checked);
        else if (!!tr.salesPoints && !!tr.salesPoints.length) {
          tr.salesPoints.forEach(sl => {
            sl.isSelected = checked;
          })
        }
      })
    }
    fun([node], checked);
  }

  getNodeById(id: number) {
    let find = (tree: NodeTree[]) => {
      let node = tree.find(t => t.node.id === id);
      if (node) return node;
      //@ts-ignore
      else return find(tree.filter(x => !!x.nodes).map(x => x.nodes).flat());
    }
    return find(this.nodeTree);

  }

  handleBack(){
    this.router.navigate([RoutesGuidelineModule.Parent,RoutesGuidelineModule.GuidelineSetup]);
  }

}
