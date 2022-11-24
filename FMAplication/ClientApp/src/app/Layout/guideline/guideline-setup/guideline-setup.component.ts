import { IDateRange } from './../../../Shared/interfaces/IDateRange';
import { GuidelineSetupService } from './../../../Shared/Services/GuidelineSetup/guidelineSetup.service';
import { UserService } from 'src/app/Shared/Services/Users';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit, ViewChild } from '@angular/core';
import { IPTableSetting, colDef } from 'src/app/Shared/Modules/p-table';
import { GuidelineSetupTableData, GuidelineSetup } from 'src/app/Shared/Entity/Guidelines/guideline-setup';
import { IDropdown } from 'src/app/Shared/interfaces/IDropdown';
import { SalesPoint } from 'src/app/Shared/Entity';
import { Utility } from 'src/app/Shared/utility';
import { TaskAssignedUserType } from 'src/app/Shared/Enums';
import { Paginator } from 'primeng/paginator';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';

@Component({
  selector: 'app-guideline-setup',
  templateUrl: './guideline-setup.component.html',
  styleUrls: ['./guideline-setup.component.css']
})
export class GuidelineSetupComponent implements OnInit {

  pageIndex = 1;
  pageSize = 10;
  total: number;
  salesPointDropdownData : IDropdown[] = [];
  guidelineSetups: GuidelineSetup[] = [];
  guidelineSetupsTableData: GuidelineSetupTableData[] = [];
  search: string = "";

  @ViewChild("paging", { static: false }) paging: Paginator;


  date = new Date();
  initialYearAdjustCount=100;
  fromDate = new Date(this.date.getFullYear(), this.date.getMonth() , 1).toISOString();
  toDate=  new Date(this.date.getFullYear(), this.date.getMonth()+1 , 0 ,23,59,59).toISOString();

  public ptableSettings:IPTableSetting<colDef<keyof GuidelineSetupTableData>> = {
    tableID: "Guideline-setup-table",
    tableClass: "table-responsive",
    tableName: 'Guideline Setup List',
    tableRowIDInternalName: "Id",
    tableColDef: [
      { headerName: 'Sales point code', width: '15%', internalName: 'salesPointCode', sort: true, type: ""},
      { headerName: 'Salespoint name', width: '15%', internalName: 'salesPointName', sort: true, type: "" },
      { headerName: 'Start Date', width: '13%', internalName: 'startDate', sort: true, type: "" },
      { headerName: 'End Date', width: '12%', internalName: 'endDate', sort: true, type: "" },
      { headerName: 'Status', width: '10%', internalName: 'status', sort: true, type: "" },
      { headerName: 'Guideline Text', width: '15%', internalName: 'guidelineName', sort: false, type: "html" },
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
    selectedDropdownValue:0,
    dropdownData:  this.salesPointDropdownData, 
    dropdownLabel: "SalesPoint",
  };

  permissionGroup: PermissionGroup = new PermissionGroup();
    
  constructor(private router:Router, private userService:UserService, 
    private guidelineSetupService:GuidelineSetupService,
    private activatedRoute: ActivatedRoute, private activityPermissionService: ActivityPermissionService) {
      this.initPermissionGroup();
     }

  private initPermissionGroup() {
    this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
    this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
    this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
  }

  ngOnInit() {
    this.getGuidelines();
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
    return Utility.getPaginationStatus(this.total, this.pageIndex, this.pageSize,this.guidelineSetupsTableData.length);
  }


  getGuidelines(){
    this.guidelineSetupService.getAll({pageIndex:this.pageIndex,pageSize:this.pageSize,
    search: this.search, fromDateTime: this.fromDate,
    toDateTime:this.toDate,salesPointId:this.ptableSettings.selectedDropdownValue}).subscribe((res) => {
      this.guidelineSetups = res.data.data;
      this.total = res.data.count;
      this.mapTableData();
      this.paging.updatePaginatorState();
    });
  }

  mapTableData(){
    const rows:GuidelineSetupTableData[]=[];
    this.guidelineSetups.map(gs=>{
      const row = new GuidelineSetupTableData();
      row.endDate = Utility.getDateToStringFormat(gs.toDateStr);
      row.startDate = Utility.getDateToStringFormat(gs.fromDateStr);
      row.status = (gs.status == 1? "Active": "InActive");
      row.disableEdit = (gs.status == 0);

      if(gs.guidelineText) row.guidelineName = gs.guidelineText;
      if(gs.salesPoint) {
        row.salesPointName = gs.salesPoint.name;
        row.salesPointCode = gs.salesPoint.code;
      }
      row.userType = TaskAssignedUserType[gs.userType];
      row.id = gs.id;
      rows.push(row);
    });
    
    this.guidelineSetupsTableData = rows;

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

  handleCustomActivityOnRecord({ action: action, record: recordInfo }){
    console.log(action,recordInfo);
    if(action === "view-item"){
      
    }
    if (action == "new-record") {
      this.router.navigate(['/guideline/new-guideline-setup']);
    }
    else if (action == "edit-item") {
      // this.edit(event.record.id);
      const record:GuidelineSetupTableData = recordInfo;
      this.router.navigate(['/guideline/edit-guideline-setup',record.id]);
    }
  }

  fnSearch($event: any) {
    this.search = $event.searchVal;
    this.paging.changePage(0);
  }
  dropDownChange(selected:number){
    this.paging.changePage(0);
  }

  handlePageSizeChange(pageSize:number){
    this.pageSize = pageSize;
    this.paging.changePage(0);   
  }

  paginate(event) {    
    this.pageIndex = Number(event.page) + 1;                    
    this.getGuidelines();
    this.ptableSettings.serverSitePageIndex = this.pageIndex;
  }

  handleDateRange(dateRange:IDateRange){
    Utility.adjustDateRange(dateRange);        
    this.fromDate = dateRange.from;
    this.toDate = dateRange.to;
    this.paging.changePage(0); 
  }

}
