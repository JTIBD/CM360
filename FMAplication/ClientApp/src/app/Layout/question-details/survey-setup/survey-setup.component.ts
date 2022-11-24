import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Paginator } from 'primeng/paginator';
import { SalesPoint, Survey, SurveyTableData } from 'src/app/Shared/Entity';
import { TaskAssignedUserTypeStrs } from 'src/app/Shared/Enums';
import { IDateRange } from 'src/app/Shared/interfaces';
import { IDropdown } from 'src/app/Shared/interfaces/IDropdown';
import { colDef, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { SurveyService } from 'src/app/Shared/Services/Question-Details/survey.service';
import { UserService } from 'src/app/Shared/Services/Users';
import { Utility } from 'src/app/Shared/utility';

@Component({
  selector: 'app-survey-setup',
  templateUrl: './survey-setup.component.html',
  styleUrls: ['./survey-setup.component.css']
})
export class SurveySetupComponent implements OnInit {
  permissionGroup: PermissionGroup = new PermissionGroup();

  surveylist:SurveyTableData[]=[];
  surveys:Survey[]=[];
  dropdownData : IDropdown[] = [];

  search: string = "";
  pageIndex = 1;
  pageSize = 10;
  total: number;

  date = new Date();
  initialYearAdjustCount=0;

  fromDate = Utility.getFirstDateOfCurrentMonth();
  toDate=  Utility.getLastDateOfCurrentMonth();

  selectedSurvey:Survey;

  @ViewChild("paging", { static: false }) paging: Paginator;
  // showingPageDetails:any;

  public ptableSettings:IPTableSetting<colDef<keyof SurveyTableData>> = {
    tableID: "Survey-table2",
    tableClass: "table-responsive",
    tableName: 'Survey List',
    tableRowIDInternalName: "Id",
    tableColDef: [
      { headerName: 'Sales point code', width: '10%', internalName: 'salesPointCode', sort: true, type: "" },
      { headerName: 'Salespoint name', width: '10%', internalName: 'salesPointName', sort: true, type: "" },
      { headerName: 'Start Date', width: '15%', internalName: 'startDate', sort: true, type: "" },
      { headerName: 'End Date', width: '10%', internalName: 'endDate', sort: true, type: "" },
      { headerName: 'Status', width: '10%', internalName: 'status', sort: true, type: "" },
      { headerName: 'Question Set', width: '15%', internalName: 'questionSet', sort: true, type: "" },
      { headerName: 'Survey Type', width: '15%', internalName: 'surveyType', sort: true, type: "" },
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

  constructor(private router: Router,private surveyService:SurveyService,
    private userService:UserService, private activatedRoute: ActivatedRoute, private activityPermissionService: ActivityPermissionService) { 
      this.initPermissionGroup();
  }

  private initPermissionGroup() {
    this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
    this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
    this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
  }

  ngOnInit() {
    this.getSurveys();
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
    return Utility.getPaginationStatus(this.total, this.pageIndex, this.pageSize,this.surveylist.length);
  }



  getSurveys(){
    this.surveyService.getSurveys(this.pageIndex,this.pageSize,this.search,this.fromDate,this.toDate,this.ptableSettings.selectedDropdownValue).subscribe(res=>{
      this.surveys = res.data;
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
    const rows:SurveyTableData[]=[];
    this.surveys.forEach(s=>{
      const row = new SurveyTableData();
      row.endDate = Utility.getDateToStringFormat(s.toDateStr);
      row.startDate = Utility.getDateToStringFormat(s.fromDateStr);
      row.status = (s.status == 1? "Active": "InActive");
      row.disableEdit = (s.status == 0);

      if(s.surveyQuestionSet) row.questionSet = s.surveyQuestionSet.name;
      if(s.salesPoint) {
        row.salesPointName = s.salesPoint.name;
        row.salesPointCode = s.salesPoint.code;
      }
      row.userType = TaskAssignedUserTypeStrs[s.userType];
      row.surveyId = s.id;
      row.surveyType = s.isConsumerSurvey?"Consumer":"Customer";
      rows.push(row);
    });
    this.surveylist = rows;

  }
  
  createNewSurvey() {
    this.router.navigate(['/question/survey-setup/new']);
  }
  edit(id: number) {
    console.log("Survey Id:", id);
    //this.router.navigate([`/question/survey-generation/${id}`]);
  }

  fnTransactionPtableCellClick(event){
    if(event.cellName !=="details") return;
    console.log(event);
    let row:SurveyTableData = event.record;
    this.selectedSurvey = this.surveys.find(x=>x.id === row.surveyId);
    if(!this.selectedSurvey) return;
    //this.mapSelectedTransactionToDetailsTableData();
    //this.selectedView = "transactionDetails";
  }

  haneleCustomActivityOnRecord({ action: action, record: recordInfo }){
    console.log(action,recordInfo);
    if(action === "view-item"){
      
    }
    if (action == "new-record") {
      this.createNewSurvey();
    }
    else if (action == "edit-item") {
      // this.edit(event.record.id);
      const record:SurveyTableData = recordInfo;
      this.router.navigate(['/question/survey-setup/edit',record.surveyId]);
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
    this.getSurveys();
    this.ptableSettings.serverSitePageIndex = this.pageIndex;
  }

  handleDateRange(dateRange:IDateRange){
    Utility.adjustDateRange(dateRange);        
    this.fromDate = dateRange.from;
    this.toDate = dateRange.to;
    this.paging.changePage(0); 
  }

}
