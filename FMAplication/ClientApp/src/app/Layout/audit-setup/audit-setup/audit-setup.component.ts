import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Paginator } from 'primeng/paginator';
import { SalesPoint } from 'src/app/Shared/Entity';
import { AuditSetup, AuditSetupTableData } from 'src/app/Shared/Entity/Daily-Audit';
import { TaskAssignedUserTypeStrs } from 'src/app/Shared/Enums';
import { ActionType, ActionTypeLabel } from 'src/app/Shared/Enums/actionType';
import { IDateRange } from 'src/app/Shared/interfaces';
import { IDropdown } from 'src/app/Shared/interfaces/IDropdown';
import { IPTableSetting, colDef } from 'src/app/Shared/Modules/p-table';
import { RoutesLaout } from 'src/app/Shared/Routes/RoutesLaout';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { DailyAuditService } from 'src/app/Shared/Services/DailyActivity/daily-audit.service';
import { UserService } from 'src/app/Shared/Services/Users';
import { Utility } from 'src/app/Shared/utility';
import { RoutesAudit } from '../routesAudit';

@Component({
  selector: 'app-audit-setup',
  templateUrl: './audit-setup.component.html',
  styleUrls: ['./audit-setup.component.css']
})
export class AuditSetupComponent implements OnInit {

  auditlist:AuditSetupTableData[]=[];
  audits:AuditSetup[]=[];
  dropdownData : IDropdown[] = [];

  search: string = "";
  pageIndex = 1;
  pageSize = 10;
  total: number=0;

  date = new Date();
  initialYearAdjustCount=0;

  fromDate = Utility.getFirstDateOfCurrentMonth();
  toDate=  Utility.getLastDateOfCurrentMonth();


  selectedAuditSetup:AuditSetup;

  @ViewChild("paging", { static: false }) paging: Paginator;

  public ptableSettings:IPTableSetting<colDef<keyof AuditSetupTableData>> = {
    tableID: "AuditSetup-table2",
    tableClass: "table-responsive",
    tableName: 'Audit Setup List',
    tableRowIDInternalName: "Id",
    tableColDef: [
      { headerName: 'Sales point code', width: '15%', internalName: 'salesPointCode', sort: true, type: "" },
      { headerName: 'Salespoint name', width: '15%', internalName: 'salesPointName', sort: true, type: "" },
      { headerName: 'Start Date', width: '13%', internalName: 'startDate', sort: true, type: "" },
      { headerName: 'End Date', width: '12%', internalName: 'endDate', sort: true, type: "" },
      { headerName: 'Status', width: '10%', internalName: 'status', sort: true, type: "" },
      { headerName: 'Audit Type', width: '20%', internalName: 'auditType', sort: true, type: "" },
      { headerName: 'User Type', width: '10%', internalName: 'userType', sort: true, type: "" },
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
    dropdownLabel:"Salespoint",
    dropdownData:  this.dropdownData, 

  };

  permissionGroup: PermissionGroup = new PermissionGroup();
  private initPermissionGroup() {
      this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
      this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
      this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
  }

  constructor(private router: Router,private auditService: DailyAuditService, private userService:UserService,
    private activatedRoute: ActivatedRoute, private activityPermissionService: ActivityPermissionService) {
      this.initPermissionGroup(); 
  }

  ngOnInit() {
     this.getAuditSetups();
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
    return Utility.getPaginationStatus(this.total, this.pageIndex, this.pageSize,this.auditlist.length);
  }


  getAuditSetups(){
    this.auditService.getAuditSetups(this.pageIndex,this.pageSize,this.search,this.fromDate,this.toDate,this.ptableSettings.selectedDropdownValue).subscribe(res=>{
      this.audits = res.data;
      this.total = res.count;
      this.mapTableData();
      this.paging.updatePaginatorState();
    })
  }
  getSalesPoints() {
    this.userService.getAllSalesPointByCurrentUser().subscribe((res) => {
     const salesPoints:SalesPoint[] =  [...res.data];
     this.mapToDropDown(salesPoints);
     this.ptableSettings.dropdownData = this.dropdownData;
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
     this.dropdownData = [{label:"All Salespoints",value:0},...options]
     this.ptableSettings.selectedDropdownValue = this.dropdownData[0].value;

  }
  dropDownChange(selected:number){
    this.paging.changePage(0);
  }

  mapTableData(){
    const rows:AuditSetupTableData[]=[];
    this.audits.forEach(s=>{
      const row = new AuditSetupTableData();
      const auditTypes:ActionType[]=[];
      row.endDate = Utility.getDateToStringFormat(s.toDateStr);
      row.startDate = Utility.getDateToStringFormat(s.fromDateStr);
      row.status = (s.status == 1? "Active": "InActive");
      row.disableEdit = (s.status == 0);

      if(s.auditProducts){
        const types = s.auditProducts.map(x=>x.actionType).filter((x,index,arr)=> arr.indexOf(x) == index);
         auditTypes.push(...types);
      }
      if(s.auditPOSMProducts){
        const types = s.auditPOSMProducts.map(x=>x.actionType).filter((x,index,arr)=> arr.indexOf(x) == index);
        auditTypes.push(...types);
      }      
      if(s.salesPoint) {
        row.salesPointName = s.salesPoint.name;
        row.salesPointCode = s.salesPoint.code;
      }
      row.userType = TaskAssignedUserTypeStrs[s.userType];
      row.auditSetupId = s.id;
      row.auditType = auditTypes.map(x=> ActionTypeLabel.ActionType[x].label).join();
      rows.push(row);
    });
    this.auditlist = rows;

  }
  
  createNewAuditSetup() {
    // this.router.navigate([RoutesLaout.AuditSetup,RoutesAudit.create]);
    this.router.navigate([RoutesLaout.AuditSetup,RoutesAudit.create]);
    //audit-setup
  }
  edit(id: number) {
    console.log("AuditSetup Id:", id);
  }

  fnTransactionPtableCellClick(event){
    if(event.cellName !=="details") return;
    console.log(event);
    let row:AuditSetupTableData = event.record;
     this.selectedAuditSetup = this.audits.find(x=>x.id === row.auditSetupId);
     if(!this.selectedAuditSetup) return;

  }

  haneleCustomActivityOnRecord({ action: action, record: recordInfo }){
    console.log(action,recordInfo);
    if(action === "view-item"){
      
    }
    if (action == "new-record") {
      this.createNewAuditSetup();
    }
    else if (action == "edit-item") {
      const record:AuditSetupTableData = recordInfo;      
      this.router.navigate([RoutesLaout.AuditSetup,RoutesAudit.edit ,record.auditSetupId]);
    }
  }

  resetDateRange(){
     this.fromDate = Utility.getFirstDateOfCurrentMonth();
     this.toDate=  Utility.getLastDateOfCurrentMonth();
  }

  reset(){
    this.resetDateRange();    
    this.paging.changePage(0);
  }
  
  fnSearch($event: any) {
    this.search = $event.searchVal;
    this.paging.changePage(0);
  }

  handlePazeSizeChange(pageSize:number){
    this.pageSize = pageSize;
    this.paging.changePage(0);   
  }
  paginate(event) {    
    this.pageIndex = Number(event.page) + 1;                    
    this.getAuditSetups();
    this.ptableSettings.serverSitePageIndex = this.pageIndex;
  }

  handleDateRange(dateRange:IDateRange){
    Utility.adjustDateRange(dateRange);        
    this.fromDate = dateRange.from;
    this.toDate = dateRange.to;
    this.paging.changePage(0); 
  }

}
