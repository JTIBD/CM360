import { Component, OnInit } from '@angular/core';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { first } from 'rxjs/operators';
import { NodeTree, SalesPoint} from 'src/app/Shared/Entity';
import { TaskAssignedUserType, TaskAssignedUserTypeStrs } from 'src/app/Shared/Enums';
import { IDateRange } from 'src/app/Shared/interfaces';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { UserService } from 'src/app/Shared/Services/Users';
import { Utility } from 'src/app/Shared/utility';
import * as moment from 'moment';
import { Router } from '@angular/router';
import { AvCommunicationService } from 'src/app/Shared/Services/AvCommunication/avCommunication.service';
import { AvSetup } from 'src/app/Shared/Entity/AVCommunications/avSetup';
import { AvCommunication, CampaignType } from 'src/app/Shared/Entity/AVCommunications/avCommunication';
import { RoutesAvCommunication } from 'src/app/Shared/Routes/RoutesAvCommunication';

@Component({
  selector: 'app-new-av',
  templateUrl: './new-av-setup.component.html',
  styleUrls: ['./new-av-setup.component.css']
})
export class NewAvSetupComponent implements OnInit {

  public selectedFromDate: NgbDateStruct;
  public selectedToDate: NgbDateStruct;
  public minDate:NgbDateStruct;
  selectedAVId:number;
  selectedUserType:TaskAssignedUserType= TaskAssignedUserType.BOTH;
  userTypes = TaskAssignedUserTypeStrs;
  avSets:AvCommunication[]=[];
  

  nodeTree:NodeTree[]=[];

  salesPointIds:number[]=[]

  
  constructor(private commonService:CommonService,
    private avSetupService:AvCommunicationService,
    private userService:UserService,
    private alertService:AlertService,
    private router: Router,
    //private avService:AvCoomunicationService
    ) { }

  ngOnInit() {
    this.minDate = this.commonService.dateToNgbDate(new Date());
    this.avSetupService.getAll().subscribe((res)=>{
      this.avSets = res.filter(x=>x.campaignType === CampaignType.Video);
    })

    this.userService.getNodeTreeByCurrentUser().subscribe(data=>{
      this.nodeTree = data;
    })
  }

  handleFromDateChange(){
    let from = this.commonService.ngbDateToDate(this.selectedFromDate).toISOString();
    let toDate = this.commonService.ngbDateToDate(this.selectedToDate).toISOString();
    if(moment(from).isAfter(toDate,"date") ) {
      this.selectedToDate = null;
    }
  }
  handleToDateChange(){    
    let from = this.commonService.ngbDateToDate(this.selectedFromDate).toISOString();
    let toDate = this.commonService.ngbDateToDate(this.selectedToDate).toISOString();
    if(moment(from).isAfter(toDate,"date") ) {
      this.selectedFromDate = null;
    }
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
  
  submit(){
    const avSetupList:AvSetup[]=[];
    const fromDateStr = this.commonService.ngbDateToDate(this.selectedFromDate).toISOString();
    const toDateStr = this.commonService.ngbDateToDate(this.selectedToDate).toISOString();
    const dateRange:IDateRange={
      from:fromDateStr,
      to:toDateStr,
    };
    Utility.adjustDateRange(dateRange);
    const salesPoints = this.getSelectedSalesPoints();
    if(!salesPoints.length) {
      this.alertService.tosterDanger("No salespoint selected");
      return;
    }
    salesPoints.forEach(sp=>{
      const av = new AvSetup();
      av.code="SV_"+sp.code;
      av.fromDate=dateRange.from;
      av.toDate = dateRange.to;
      av.salesPointId = sp.salesPointId;
      av.avId = this.selectedAVId;
      av.userType = this.selectedUserType;
      avSetupList.push(av);
    });
    this.avSetupService.getExistingAvSetup({data:avSetupList}).pipe(first()).subscribe(res=>{
      if(res.length) {
        if(this.alertService.loadingFlag) this.alertService.fnLoading(false);
        let sps = salesPoints.filter(x=> res.some(sv=>sv.salesPointId == x.salesPointId)).filter((sp,i,arr)=> arr.findIndex(x=>x.salesPointId == sp.salesPointId) === i);
        let spNames = sps.map(x=>x.name);

        this.alertService.confirm(`AvSetup already exist in Salespoint ${spNames.join(", ")}. Do you want to stop the avSetups before the new av starts?`, () => {
          this.avSetupService.createNewAvSetup({ data: avSetupList }).subscribe(res => {
            console.log(res);
            this.alertService.tosterSuccess("AvSetups created successfully");
            this.router.navigate(["/av-communication/av-setups"]);
          });
        },()=>{}); 

      }
      else{
        this.avSetupService.createNewAvSetup({ data: avSetupList }).subscribe(res => {
          console.log(res);
          this.alertService.tosterSuccess("AvSetups created successfully");
          this.router.navigate(["/av-communication/av-setups"]);
        });
      }
    })

  }
  handleSalesPointSelect(event,salesPoint:SalesPoint){
    salesPoint.isSelected = event.target.checked;
    // if(event.target.checked) this.selectedSalesPointIds.push(salesPointId);
    // else this.selectedSalesPointIds = this.selectedSalesPointIds.filter(x=> x !== salesPointId);
  }

  handleNodeSelect(item:NodeTree,checked:boolean){
    console.log(item,checked);
    const node:NodeTree = this.getNodeById(item.node.id);
    console.log(node);
    if(!node) return;
    let fun = (trees:NodeTree[],checked:boolean)=>{
      trees.forEach(tr=>{
        tr.isSelected = checked;
        if(!!tr.nodes && tr.nodes.length) fun(tr.nodes,checked);
        else if(!!tr.salesPoints && !!tr.salesPoints.length){
          tr.salesPoints.forEach(sl=>{
            sl.isSelected = checked;
          })
        }
      })
    }
    fun([node],checked);
  }

  getNodeById(id:number){
    let find=(tree:NodeTree[])=>{
      let node = tree.find(t=>t.node.id === id);
      if(node) return node;
      //@ts-ignore
      else return find(tree.filter(x=>!!x.nodes).map(x=>x.nodes).flat());
    }
    return find(this.nodeTree);
  
  }

  handleBack(){
    this.router.navigate([RoutesAvCommunication.Parent, RoutesAvCommunication.AvSetup]);
  }

}

