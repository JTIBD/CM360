import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { first } from 'rxjs/operators';
import { NodeTree, SalesPoint } from 'src/app/Shared/Entity';
import { AvCommunication, CampaignType } from 'src/app/Shared/Entity/AVCommunications/avCommunication';
import { AvSetup } from 'src/app/Shared/Entity/AVCommunications/avSetup';
import { TaskAssignedUserType, TaskAssignedUserTypeStrs } from 'src/app/Shared/Enums';
import { IDateRange } from 'src/app/Shared/interfaces';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { AvCommunicationService } from 'src/app/Shared/Services/AvCommunication/avCommunication.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { UserService } from 'src/app/Shared/Services/Users';
import { Utility } from 'src/app/Shared/utility';
import * as moment from 'moment';
import { CommunicationSetup } from 'src/app/Shared/Entity/AVCommunications/communicationSetup';


@Component({
  selector: 'app-add-communication-setup',
  templateUrl: './add-communication-setup.component.html',
  styleUrls: ['./add-communication-setup.component.css']
})
export class AddCommunicationSetupComponent implements OnInit {

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
    private avService:AvCommunicationService,
    private userService:UserService,
    private alertService:AlertService,
    private router: Router,
    //private avService:AvCoomunicationService
    ) { }

  ngOnInit() {
    this.minDate = this.commonService.dateToNgbDate(new Date());
    this.avService.getAll().subscribe((res)=>{
      this.avSets = res.filter(x=>x.campaignType === CampaignType.Image);
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
    const commnucationList:CommunicationSetup[]=[];
    const fromDateStr = this.commonService.ngbDateToDate(this.selectedFromDate).toISOString();
    const toDateStr = this.commonService.ngbDateToDate(this.selectedToDate).toISOString();
    const dateRange:IDateRange={
      from:fromDateStr,
      to:toDateStr,
    };
    Utility.adjustDateRange(dateRange);
    const salesPoints = this.getSelectedSalesPoints();
    if(!salesPoints.length) this.alertService.tosterDanger("No salespoint selected");
    salesPoints.forEach(sp=>{
      const av = new CommunicationSetup();
      av.code="SV_"+sp.code;
      av.fromDate=dateRange.from;
      av.toDate = dateRange.to;
      av.salesPointId = sp.salesPointId;
      av.avCommunicationId = this.selectedAVId;
      av.userType = this.selectedUserType;
      commnucationList.push(av);
    });
    this.avService.getExistingCommunicationSetup({data:commnucationList}).pipe(first()).subscribe(res=>{
      if(res.length) {
        if(this.alertService.loadingFlag) this.alertService.fnLoading(false);
        let sps = salesPoints.filter(x=> res.some(sv=>sv.salesPointId == x.salesPointId)).filter((sp,i,arr)=> arr.findIndex(x=>x.salesPointId == sp.salesPointId) === i);
        let spNames = sps.map(x=>x.name);

        this.alertService.confirm(`Communication setup  already exist in Salespoint ${spNames.join(", ")}. Do you want to stop the Communication setup before the new communication starts?`, () => {
          this.avService.createNewCommunicationsetup({ data: commnucationList }).subscribe(res => {
            console.log(res);
            this.alertService.tosterSuccess("Communication setup created successfully");
            this.router.navigate(["/av-communication/communication-setup-list"]);
          });
        },()=>{}); 

      }
      else{
        this.avService.createNewCommunicationsetup({ data: commnucationList }).subscribe(res => {
          console.log(res);
          this.alertService.tosterSuccess("Communication setup created successfully");
          this.router.navigate(["/av-communication/communication-setup-list"]);
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

}

