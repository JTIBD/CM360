import { MinimumExecutionLimitService } from './../../../Shared/Services/minimum-execution-limit/minimum-execution-limit.service';
import { Router } from '@angular/router';
import { UserService } from './../../../Shared/Services/Users/user.service';
import { MinimumExecutionLimit, MinimumExecutionLimitTableData } from './../../../Shared/Entity/minimum-execution-limit/minimum-execution-limit';
import { IDropdown } from './../../../Shared/interfaces/IDropdown';
import { Component, OnInit, ViewChild } from '@angular/core';
import { IPTableSetting, colDef } from 'src/app/Shared/Modules/p-table';
import { SalesPoint } from 'src/app/Shared/Entity';
import { Paginator } from 'primeng/paginator';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { Utility } from 'src/app/Shared/utility';

@Component({
  selector: 'app-execution-limit-list',
  templateUrl: './execution-limit-list.component.html',
  styleUrls: ['./execution-limit-list.component.css']
})
export class ExecutionLimitListComponent implements OnInit {

  pageIndex = 1;
  pageSize = 10;
  total:number;
  salesPointDropdownData : IDropdown[] = [];
  minimumExecutionLimits: MinimumExecutionLimit[] = [];
  minimumExecutionLimitTableData: MinimumExecutionLimitTableData[] = [];
  search:string = "";
  @ViewChild("paging", { static: false }) paging: Paginator;

  public ptableSettings:IPTableSetting<colDef<keyof MinimumExecutionLimitTableData>> = {
    tableID: "minimum-execution-limit-table",
    tableClass: "table-responsive",
    tableName: 'Minimum Execution Limit List',
    tableRowIDInternalName: "Id",
    tableColDef: [
      { headerName: 'Sales point code', width: '30%', internalName: 'salesPointCode', sort: true, type: ""},
      { headerName: 'Salespoint name', width: '30%', internalName: 'salesPointName', sort: true, type: "" },
      { headerName: 'Target Visited Outlet', width: '30%', internalName: 'targetVisitedOutlet', sort: true, type: "" },
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: this.pageSize,
    enabledPagination: false,
    enabledEditBtn: true,
	  enabledDeleteBtn: true,
    enabledColumnFilter: true,
    enabledRecordCreateBtn: true,
    enabledDataLength:true,
    enableDateRangeFilter:false,
    enabledServerSitePaggination: true,
    tableFooterVisibility:false,
    enablePazeSizeSelection:true,
    enableDropdownFilter:true,
    selectedDropdownValue:0,
    dropdownData:  this.salesPointDropdownData, 
    dropdownLabel: "SalesPoint",
  };

  constructor(private userService:UserService, private router: Router, 
    private minimumExecutionLimitService:MinimumExecutionLimitService,
    private alertService: AlertService) { }

  ngOnInit() {
    this.getSalesPoints();
    this.getTargetOutlets();
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
    return Utility.getPaginationStatus(this.total, this.pageIndex, this.pageSize,this.minimumExecutionLimitTableData.length);
  }


  getTargetOutlets(){
    this.minimumExecutionLimitService.getAll(
      {pageIndex:this.pageIndex,pageSize:this.pageSize,
    searchText: this.search,salesPointId:this.ptableSettings.selectedDropdownValue})
    .subscribe((res) => {
      this.minimumExecutionLimits = res.data.data;
      this.total = res.data.count;
      this.mapTableData();
    });
  }

  mapTableData(){
    const rows: MinimumExecutionLimitTableData[]=[];
    this.minimumExecutionLimits.map(gs=>{
      const row = new MinimumExecutionLimitTableData();

      if(gs.targetVisitedOutlet) row.targetVisitedOutlet = gs.targetVisitedOutlet;
      if(gs.salesPoint) {
        row.salesPointName = gs.salesPoint.name;
        row.salesPointCode = gs.salesPoint.code;
      }
      row.id = gs.id;
      rows.push(row);
    });
    
    this.minimumExecutionLimitTableData = rows;
  }

  private delete(id: number) {
    this.alertService.confirm("Are you sure you want to delete this item?", () => {
        this.minimumExecutionLimitService.delete(id).subscribe(
            (res: any) => {
                this.alertService.tosterSuccess("Target visited outlet has been deleted successfully.");
                this.getTargetOutlets();
            },
            (error) => {
                console.log(error);
                this.alertService.tosterDanger("Failed to delete.");
            }
        );
    }, () => {

    });
}

  handleCustomActivityOnRecord({ action: action, record: recordInfo }){
    console.log(action,recordInfo);
    const record:MinimumExecutionLimitTableData = recordInfo;

    if (action == "delete-item") {
      this.delete(record.id);
  }
    if (action == "new-record") {
      this.router.navigate(['/configuration/new-execution-limit']);
    }
    else if (action == "edit-item") {
      this.router.navigate(['/configuration/edit-execution-limit',record.id]);
    }
  }

  fnSearch($event: any) {
    this.search = $event.searchVal;
    this.paging.changePage(0);
  }

  dropDownChange(selected:number){
    this.paging.changePage(0);
  }

  paginate(event) {    
    this.pageIndex = Number(event.page) + 1;                    
    this.getTargetOutlets();
    this.ptableSettings.serverSitePageIndex = this.pageIndex;
  }

  handlePageSizeChange(pageSize:number){
    this.pageSize = pageSize;
    this.paging.changePage(0);   
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

}
