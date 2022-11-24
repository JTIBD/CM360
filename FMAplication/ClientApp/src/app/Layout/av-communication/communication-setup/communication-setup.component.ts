import { CommunicationSetup, CommunicationSetupTableData } from './../../../Shared/Entity/AVCommunications/communicationSetup';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Paginator } from 'primeng/paginator';
import { SalesPoint } from 'src/app/Shared/Entity';
import { TaskAssignedUserType } from 'src/app/Shared/Enums';
import { IDateRange } from 'src/app/Shared/interfaces';
import { IDropdown } from 'src/app/Shared/interfaces/IDropdown';
import { colDef, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { AvCommunicationService } from 'src/app/Shared/Services/AvCommunication/avCommunication.service';
import { UserService } from 'src/app/Shared/Services/Users';
import { Utility } from 'src/app/Shared/utility';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { RoutesLaout } from 'src/app/Shared/Routes/RoutesLaout';
import { RoutesAvCommunication } from 'src/app/Shared/Routes/RoutesAvCommunication';

@Component({
  selector: 'app-communication-setup',
  templateUrl: './communication-setup.component.html',
  styleUrls: ['./communication-setup.component.css']
})
export class CommunicationSetupComponent implements OnInit {

  communicationlist:CommunicationSetupTableData[]=[];
  communicationSetups:CommunicationSetup[]=[];

  search: string = "";
  pageIndex = 1;
  pageSize = 10;
  total: number;

  date = new Date();
  initialYearAdjustCount=100;

  fromDate = new Date(this.date.getFullYear(), this.date.getMonth() , 1).toISOString();
  toDate=  new Date(this.date.getFullYear(), this.date.getMonth()+1 , 0 ,23,59,59).toISOString();

  selectedCommunication:CommunicationSetup;
  salesPointDropdownData : IDropdown[] = [];

  @ViewChild("paging", { static: false }) paging: Paginator;
  // showingPageDetails:any;

  public ptableSettings:IPTableSetting<colDef<keyof CommunicationSetupTableData>> = {
    tableID: "Communication-setup-table",
    tableClass: "table-responsive",
    tableName: 'Communication Setup List',
    tableRowIDInternalName: "Id",
    tableColDef: [
      { headerName: 'Sales point code', width: '15%', internalName: 'salesPointCode', sort: true, type: "" },
      { headerName: 'Salespoint name', width: '15%', internalName: 'salesPointName', sort: true, type: "" },
      { headerName: 'Start Date', width: '13%', internalName: 'startDate', sort: true, type: "" },
      { headerName: 'End Date', width: '12%', internalName: 'endDate', sort: true, type: "" },
      { headerName: 'Status', width: '10%', internalName: 'status', sort: true, type: "" },
      { headerName: 'Communication', width: '10%', internalName: 'avName', sort: true, type: "" },
      { headerName: 'User Type', width: '15%', internalName: 'userType', sort: true, type: "" },
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: this.pageSize,
    enabledPagination: false,
    enabledEditBtn: true,
	  // enabledDeleteBtn: true,
    enabledColumnFilter: true,
    enabledRecordCreateBtn: true,
    enabledDataLength:true,
    enableDateRangeFilter:true,
    enabledServerSitePaggination: true,
    tableFooterVisibility:false,
    enablePazeSizeSelection:true,
    intialDateRange:{
      from:this.fromDate,
      to:this.toDate,
    },
    enableDropdownFilter:true,
    selectedDropdownValue:0,
    dropdownData:  this.salesPointDropdownData, 
    dropdownLabel: "SalesPoint", 

  };

  permissionGroup: PermissionGroup = new PermissionGroup();
  private initPermissionGroup() {
      this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
      this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
      this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
  }

  constructor(private router: Router,private avService:AvCommunicationService,private userService: UserService,
    private activatedRoute: ActivatedRoute, private activityPermissionService: ActivityPermissionService) { 
    //let dt = new Date(this.toDate);
    //dt.setFullYear(this.date.getFullYear()+this.initialYearAdjustCount);
    //this.toDate = dt.toISOString();
    this.initPermissionGroup();
  }

  ngOnInit() {
    this.getCommunications();
    this.getSalesPoints();
  }

  ngAfterViewInit() {
    this.enableCurrentPageReport();
  }

  enableCurrentPageReport() {
    let timer = setInterval(() => {
      if (this.paging) {
        this.paging.showCurrentPageReport = true;
        clearInterval(timer);
      }
    }, 1);
  }

  getPaginationStatus() {
    return Utility.getPaginationStatus(this.total, this.pageIndex, this.pageSize,this.communicationlist.length);
  }

  getCommunications(){
    this.avService.getCommunicationSetups(this.pageIndex,this.pageSize,this.search,this.fromDate,this.toDate,this.ptableSettings.selectedDropdownValue).subscribe(res=>{
      this.communicationSetups = res.data;
      this.total = res.count;
      this.mapTableData();
      this.paging.updatePaginatorState();
    })
  }

  mapTableData(){
    const rows:CommunicationSetupTableData[]=[];
    this.communicationSetups.forEach(s=>{
      const row = new CommunicationSetupTableData();
      row.endDate = Utility.getDateToStringFormat(s.toDateStr);
      row.startDate = Utility.getDateToStringFormat(s.fromDateStr);
      row.status = (s.status == 1? "Active": "InActive");
      row.disableEdit = (s.status == 0);
      if(s.avCommunication) row.avName = s.avCommunication.campaignName;
      if(s.salesPoint) {
        row.salesPointName = s.salesPoint.name;
        row.salesPointCode = s.salesPoint.code;
      }
      row.userType = TaskAssignedUserType[s.userType];
      row.id = s.id;
      rows.push(row);
    });
    this.communicationlist = rows;

  }
  getSalesPoints() {
    this.userService.getAllSalesPointByCurrentUser().subscribe((res) => {
     const salesPoints:SalesPoint[] =  [...res.data];
     this.mapToDropDown(salesPoints);
     this.ptableSettings.dropdownData = this.salesPointDropdownData;
     this.ptableSettings.selectedDropdownValue = this.salesPointDropdownData[0].value;
    });
  }

  mapToDropDown(data:SalesPoint[]) {
    let options = data.map(x => {
      const dropdownData :IDropdown = {
        label : x.name, 
        value : x.salesPointId
      };
      return dropdownData;
    });
    this.salesPointDropdownData = [{label:"All Salespoints",value:0},...options]
  }
  dropDownChange(selected:number){
    this.getCommunications();
  }
  createNew() {
    this.router.navigate(['/av-communication/add-communication-setup']);
  }
 

  fnTransactionPtableCellClick(event){
    if(event.cellName !=="details") return;
    console.log(event);
    let row:CommunicationSetupTableData = event.record;
    this.selectedCommunication = this.communicationSetups.find(x=>x.id === row.id);
    if(!this.selectedCommunication) return;
    //this.mapSelectedTransactionToDetailsTableData();
    //this.selectedView = "transactionDetails";
  }

  haneleCustomActivityOnRecord({ action: action, record: recordInfo }){
    console.log(action,recordInfo);
    if(action === "view-item"){
      
    }
    if (action == "new-record") {
      this.createNew();
    }
    else if (action == "edit-item") {
      // this.edit(event.record.id);
      const record:CommunicationSetupTableData = recordInfo;
      this.router.navigate([RoutesLaout.AvCommunication,RoutesAvCommunication.EditCommunicationSetup,record.id]);
    }
  }

  resetDateRange(){
    const dt = new Date();
    dt.setFullYear(dt.getFullYear()+this.initialYearAdjustCount);
    this.toDate = dt.toISOString();
    const fromDt = new Date();
    fromDt.setFullYear(fromDt.getFullYear()-this.initialYearAdjustCount);
    this.fromDate = fromDt.toISOString();
  }

  reset(){
    this.resetDateRange();
    this.pageIndex = 1;
    this.paging.changePage(0);
  }
  
  fnSearch($event: any) {
    this.search = $event.searchVal;
    this.paging.changePage(0);
  }

  handlePazeSizeChange(pageSize:number){
    console.log(pageSize);
    this.pageIndex=1;
    this.pageSize = pageSize;
    this.paging.changePage(0);   
  }
  paginate(event) {    
    this.pageIndex = Number(event.page) + 1;                    
    this.getCommunications();
    this.ptableSettings.serverSitePageIndex = this.pageIndex;
  }

  handleDateRange(dateRange:IDateRange){
    Utility.adjustDateRange(dateRange);        
    this.fromDate = dateRange.from;
    this.toDate = dateRange.to;
    this.paging.changePage(0); 
  }

}

