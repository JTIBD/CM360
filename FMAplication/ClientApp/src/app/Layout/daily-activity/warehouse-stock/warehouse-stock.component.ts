import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Paginator } from 'primeng/paginator';
import { WareHouse } from 'src/app/Shared/Entity/Inventory';
import { WareHouseStock, WareHouseStockTableData } from 'src/app/Shared/Entity/wareHouse';
import { IDropdown } from 'src/app/Shared/interfaces/IDropdown';
import { colDef, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { WareHouseService } from 'src/app/Shared/Services/DailyActivity/warehouse.service';
import { ReportService } from 'src/app/Shared/Services/Reports/report-service';
import { UserService } from 'src/app/Shared/Services/Users';
import { Utility } from 'src/app/Shared/utility';
import { InventoryManagementService } from '../../inventory-management/inventory-management.service';

@Component({
  selector: 'app-warehouse-stock',
  templateUrl: './warehouse-stock.component.html',
  styleUrls: ['./warehouse-stock.component.css']
})
export class WarehouseStockComponent implements OnInit {

  rows:WareHouseStockTableData[]=[];
  stocks:WareHouseStock[]=[];

  search: string = "";
  pageIndex = 1;
  pageSize = 10;
  total: number;

  date = new Date();
  initialYearAdjustCount=100;

  fromDate = new Date(this.date.getFullYear(), this.date.getMonth() , 1).toISOString();
  toDate=  new Date(this.date.getFullYear(), this.date.getMonth()+1 , 0 ,23,59,59).toISOString();


  selectedRow:WareHouseStock;

  dropdownData : IDropdown[] = [];
  allSelectionOption:IDropdown = {label:"All WareHouses",value:Utility.allSelectionValue};
  selectedDropdownOptionValues:number[]=[];

  @ViewChild("paging", { static: false }) paging: Paginator;
  // showingPageDetails:any;

  public ptableSettings:IPTableSetting<colDef<keyof WareHouseStockTableData>> = {
    tableID: "Warehouse-stock-table",
    tableClass: "table-responsive",
    tableName: 'Warehouse Stock List',
    tableRowIDInternalName: "Id",
    tableColDef: [
      { headerName: 'Warehouse', width: '15%', internalName: 'wareHouseName', sort: true, type: "" },      
      { headerName: 'POSM Code', width: '15%', internalName: 'posmCode', sort: true, type: "" },      
      { headerName: 'POSM Name', width: '15%', internalName: 'posmProductName', sort: true, type: "" },
      { headerName: 'Quantity', width: '15%', internalName: 'quantity', sort: true, type: "" }, 
      { headerName: 'Available', width: '15%', internalName: 'availableQuantity', sort: true, type: "" }
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
    multiSelectDropdownLabel: "WareHouse", 

  };

  constructor(private router: Router,private wareHouseService:WareHouseService,
    private userService: UserService,private inventoryManagementService:InventoryManagementService,
    private reportService:ReportService,
    private commonService:CommonService) { 
    //let dt = new Date(this.toDate);
    //dt.setFullYear(this.date.getFullYear()+this.initialYearAdjustCount);
    //this.toDate = dt.toISOString();
  }

  ngOnInit() {
    this.getWarehouses();
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
    this.wareHouseService.getStocks(this.pageIndex,this.pageSize,this.search,this.selectedDropdownOptionValues.filter(x=>x!== this.allSelectionOption.value)).subscribe(res=>{
      this.stocks = res.data;
      this.total = res.count;
      this.mapTableData();
      this.paging.updatePaginatorState();
    })
  }

  mapTableData(){
    const rows:WareHouseStockTableData[]=[];
    this.stocks.forEach(s=>{
      const row = new WareHouseStockTableData();
      row.id = s.id;
      if(s.posmProduct) {
        row.posmProductName = s.posmProduct.name;
        row.posmCode = s.posmProduct.code;
      }
      if(s.wareHouse) {
        row.wareHouseName = `${s.wareHouse.name}(${s.wareHouse.code})`;
      }
      row.quantity = s.quantity;
      if(row.quantity < 0) row.quantity = 0;       
      row.availableQuantity = s.availableQuantity;      
      if(row.availableQuantity < 0) row.availableQuantity = 0;
      rows.push(row);
    });
    this.rows = rows;

  }
  getWarehouses() {
    this.inventoryManagementService.getWareHouses().subscribe(data => {
      this.mapToDropDown(data);
      this.ptableSettings.multiSelectDropdownData = this.dropdownData;
      this.selectAll();
      this.getStocks();
    });    
  }

  mapToDropDown(data:WareHouse[]) {
    let options = data.map(x => {
      const dropdownData :IDropdown = {
        label : x.name, 
        value : x.id
      };
      return dropdownData;
    });
    this.dropdownData = [this.allSelectionOption,...options]
  }

  selectAll(){
    this.selectedDropdownOptionValues = this.dropdownData.map(x=>x.value);
    this.ptableSettings.multiSelectedDropdownValue = this.selectedDropdownOptionValues;
  }
  deselectAll(){
    this.selectedDropdownOptionValues = [];
    this.ptableSettings.multiSelectedDropdownValue = this.selectedDropdownOptionValues;
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
  }

  fnTransactionPtableCellClick(event){
    if(event.cellName !=="details") return;
    let row:WareHouseStockTableData = event.record;
    this.selectedRow = this.stocks.find(x=>x.id === row.id);
    if(!this.selectedRow) return;    
  }

  haneleCustomActivityOnRecord({ action: action, record: recordInfo }){
    if(action === "view-item"){
      
    }
    if (action == "new-record") {
      this.createNewAv();
    }
    else if (action == "edit-item") {
      // this.edit(event.record.id);
      const record:WareHouseStockTableData = recordInfo;
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
    this.pageIndex = 1;
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
    this.reportService.exportCWStockReportToExcel(this.selectedDropdownOptionValues).subscribe(data => {
        const fileName = `CW_Stock_Report_${Utility.getDateTimeSuffixForExcelFilename()}.xlsx`;
        this.commonService.DownloadFile(data, fileName, fileType);
    });

  }
}
