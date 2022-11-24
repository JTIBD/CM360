import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Paginator } from 'primeng/paginator';
import { SalesPoint } from 'src/app/Shared/Entity';
import { SalespointStock, SalesPointStockTableData } from 'src/app/Shared/Entity/Sales/salespointStock';
import { IDropdown } from 'src/app/Shared/interfaces/IDropdown';
import { colDef, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { ReportService } from 'src/app/Shared/Services/Reports/report-service';
import { SalesPointService } from 'src/app/Shared/Services/Sales';
import { Utility } from 'src/app/Shared/utility';


@Component({
  selector: 'app-salespoint-stock',
  templateUrl: './salespoint-stock.component.html',
  styleUrls: ['./salespoint-stock.component.css']
})
export class SalespointStockComponent implements OnInit {

 
  rows:SalesPointStockTableData[]=[];
  stocks:SalespointStock[]=[];

  search: string = "";
  pageIndex = 1;
  pageSize = 10;
  total: number;

  date = new Date();
  initialYearAdjustCount=100;

  fromDate = new Date(this.date.getFullYear(), this.date.getMonth() , 1).toISOString();
  toDate=  new Date(this.date.getFullYear(), this.date.getMonth()+1 , 0 ,23,59,59).toISOString();


  selectedRow:SalespointStock;

  dropdownData : IDropdown[] = [];
  allSelectionOption:IDropdown = {label:"All Salespoint",value:Utility.allSelectionValue};
  selectedDropdownOptionValues:number[]=[];

  @ViewChild("paging", { static: false }) paging: Paginator;
  // showingPageDetails:any;

  public ptableSettings:IPTableSetting<colDef<keyof SalesPointStockTableData>> = {
    tableID: "Salespoint-stock-table",
    tableClass: "table-responsive",
    tableName: 'Salespoint Stock List',
    tableRowIDInternalName: "Id",
    tableColDef: [
      { headerName: 'Salespoint', width: '15%', internalName: 'salesPointName', sort: true, type: "" },      
      { headerName: 'POSM Code', width: '15%', internalName: 'posmCode', sort: true, type: "" },      
      { headerName: 'POSM Name', width: '15%', internalName: 'posmProductName', sort: true, type: "" },
      { headerName: 'Quantity', width: '15%', internalName: 'quantity', sort: true, type: "" },
      { headerName: 'Available Quantity', width: '15%', internalName: 'availableQuantity', sort: true, type: "" }
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: this.pageSize,
    enabledPagination: false,
    enabledEditBtn: false,
	  // enabledDeleteBtn: true,
    enabledColumnFilter: true,
    enabledRecordCreateBtn: false,
    enabledDataLength:true,
    enabledServerSitePaggination: true,
    tableFooterVisibility:false,
    enablePazeSizeSelection:true,
    intialDateRange:{
      from:this.fromDate,
      to:this.toDate,
    },    
    enableMultiSelectDropdownFilter:true,
    multiSelectDropdownData:  this.dropdownData, 
    multiSelectDropdownLabel: "SalesPoint", 

  };

  constructor(private router: Router,
    private salesPointService:SalesPointService,
    private reportService:ReportService,
    private commonService:CommonService,
    ) { 
  }

  ngOnInit() {
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
    return Utility.getPaginationStatus(this.total, this.pageIndex, this.pageSize,this.rows.length);
  }


  getStocks(){
    this.salesPointService.getStocks(this.pageIndex,this.pageSize,this.search,this.selectedDropdownOptionValues).subscribe(res=>{
      this.stocks = res.data;
      this.total = res.count;
      this.mapTableData();
      this.paging.updatePaginatorState();
    })
  }

  mapTableData(){
    const rows:SalesPointStockTableData[]=[];
    this.stocks.forEach(s=>{
      const row = new SalesPointStockTableData();
      row.id = s.id;
      if(s.posmProduct) {
        row.posmProductName = s.posmProduct.name;
        row.posmCode = s.posmProduct.code;
      }
      if(s.salesPoint) {
        row.salesPointName = `${s.salesPoint.name}(${s.salesPoint.code})`;
      }
      row.quantity = s.quantity;
      if(row.quantity < 0) row.quantity = 0;
      row.availableQuantity = s.availableQuantity;
      if(row.availableQuantity < 0) row.availableQuantity = 0;
      rows.push(row);
    });
    this.rows = rows;

  }
  getSalesPoints() {
    this.salesPointService.getAllSalesPointByCurrentFmUser().subscribe(res => {
      this.mapToDropDown(res.data);
      this.ptableSettings.multiSelectDropdownData = this.dropdownData;
      this.selectAll();
      this.getStocks();
    });    
  }

  selectAll(){
    this.selectedDropdownOptionValues = this.dropdownData.map(x=>x.value);
    this.ptableSettings.multiSelectedDropdownValue = this.selectedDropdownOptionValues;
  }

  mapToDropDown(data:SalesPoint[]) {
    let options = data.map(x => {
      const dropdownData :IDropdown = {
        label : x.name, 
        value : x.salesPointId
      };
      return dropdownData;
    });
    this.dropdownData = [this.allSelectionOption,...options]
  }

  handleSelection(selections:IDropdown[]){
    let updatedSelections = Utility.handledAllSelection(selections,this.selectedDropdownOptionValues,this.dropdownData);
    if(updatedSelections) this.selectedDropdownOptionValues = updatedSelections;    
    else this.selectedDropdownOptionValues = selections.map(x=>x.value);
  }

  dropDownChange(selected:IDropdown[]){
    this.handleSelection(selected);
    this.ptableSettings.multiSelectedDropdownValue = this.selectedDropdownOptionValues.slice();
    this.paging.changePage(0);
  }
  createNewAv() {
    this.router.navigate(['/av-communication/new-av-setups']);
  }
  edit(id: number) {
    console.log("Av Id:", id);
    //this.router.navigate([`/question/av-generation/${id}`]);
  }

  fnTransactionPtableCellClick(event){
    if(event.cellName !=="details") return;
    let row:SalesPointStockTableData = event.record;
    this.selectedRow = this.stocks.find(x=>x.id === row.id);
    if(!this.selectedRow) return;    
  }

  haneleCustomActivityOnRecord({ action: action, record: recordInfo }){
    console.log(action,recordInfo);
    if(action === "view-item"){
      
    }
    if (action == "new-record") {
      this.createNewAv();
    }
    else if (action == "edit-item") {
      // this.edit(event.record.id);
      const record:SalesPointStockTableData = recordInfo;
      this.router.navigate(['/av-communication/edit-av-setups',record.id]);
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
    this.getStocks();
    this.ptableSettings.serverSitePageIndex = this.pageIndex;
  }

  handleExport(){
    let fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    this.reportService.exportSPStockReportToExcel(this.selectedDropdownOptionValues).subscribe(data => {
        const fileName = `SP_Stock_Report_${Utility.getDateTimeSuffixForExcelFilename()}.xlsx`;
        this.commonService.DownloadFile(data, fileName, fileType);
    });

  }

}
