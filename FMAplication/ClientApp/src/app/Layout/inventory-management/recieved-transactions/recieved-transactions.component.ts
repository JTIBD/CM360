import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Paginator } from 'primeng/paginator';
import { StockDistributionRowDetailsTableData, StockDistributionTableData } from 'src/app/Shared/Entity';
import { Transaction } from 'src/app/Shared/Entity/Inventory/Transaction';
import { StockReceiveTableData } from 'src/app/Shared/Entity/stock-distribution/stockReceiveTableData';
import { TransactionStatus, TransactionStatusStrs } from 'src/app/Shared/Enums/TransactionStatus';
import { IDateRange } from 'src/app/Shared/interfaces';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { FileUtilityService } from 'src/app/Shared/Services/fileUtility/file-utility.service';
import { Utility } from 'src/app/Shared/utility';
import { InventoryManagementService } from '../inventory-management.service';

type viewType =  "default"|"transactionDetails";

@Component({
  selector: 'app-recieved-transactions',
  templateUrl: './recieved-transactions.component.html',
  styleUrls: ['./recieved-transactions.component.css']
})
export class RecievedTransactionsComponent implements OnInit {
  
  search: string = "";
  pageIndex = 1;
  pageSize = 10;
  total: number;

  date = new Date();

  fromDate = new Date(this.date.getFullYear(), this.date.getMonth() , 1).toISOString();
  toDate=  new Date().toISOString();

  @ViewChild("paging", { static: false }) paging: Paginator;
  // showingPageDetails:any;


  public transactionPtableSettings: IPTableSetting = {
    tableID: "transactionPtable",
    tableClass: "table table-border ",
    tableName: "Stock Received Transactions",
    tableRowIDInternalName: "id",
    tableColDef: [
      { 
        headerName: "Warehouse Name", 
        width: "10%", 
        internalName: "centralWareHouseName",
        sort: true, 
        type: "",
       },
      { 
        headerName: "Sales Point Name", 
        width: "10%", 
        internalName: "salesPointName", 
        sort: true, 
        type: "",
       },
      { 
        headerName: "Transaction Number", 
        width: "15%", 
        internalName: "transactionNumber", 
        sort: true, 
        type: "",
       },
      { 
        headerName: "Chalan Number", 
        width: "15%", 
        internalName: "chalanNumber", 
        sort: true, 
        type: "",
       },
       { 
        headerName: "Line ", 
        width: "10%", 
        internalName: "lines", 
        sort: true, 
        type: "",
       },
      { 
        headerName: "Total Quantity", 
        width: "15%", 
        internalName: "quantity", 
        sort: true, 
        type: "",      
       },
       { 
        headerName: "Date", 
        width: "10%", 
        internalName: "date", 
        sort: true, 
        type: "",      
       },
       { 
        headerName: "Status ", 
        width: "10%", 
        internalName: "transactionStatus", 
        sort: true, 
        type: "",      
       },

    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: this.pageSize,
    enabledPagination: true,
    enabledColumnFilter: true,
    enabledDeleteBtn: false,
    tableFooterVisibility:false,
    enabledViewBtn:true,
    enabledCheckBtn:true,
    enabledServerSitePaggination: true,
    enablePazeSizeSelection:true,
    enabledDataLength:true,
    enableDateRangeFilter:true,
    intialDateRange:{
      from:this.fromDate,
      to:this.toDate,
    },
    
  };

  detailsTablePazeSize = 5;

  public transactionDetailsPtableSettings: IPTableSetting = {
    tableID: "transactionDetailsPtable",
    tableClass: "table table-border ",
    tableName: "Received Transaction Details",
    tableRowIDInternalName: "id",
    tableColDef: [
      { 
        headerName: "POSM Name", 
        width: "20%", 
        internalName: "posmName",
        sort: true, 
        type: "",
       },
      { 
        headerName: "Quantity", 
        width: "20%", 
        internalName: "quantity", 
        sort: true, 
        type: "",
       },
       { 
        headerName: "Received Quantity", 
        width: "20%", 
        internalName: "receivedQuantity", 
        sort: true, 
        type: "",
       },
   
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: this.detailsTablePazeSize,
    enabledPagination: true,
    enabledColumnFilter: true,
    enabledDeleteBtn: false,
    enabledPrint:false,
    
  };

  transactions: Transaction[];

  stockReceivedTableData: StockReceiveTableData[]=[];
  selectedTransaction:Transaction=null;
  
  stockDistributionRowDetailsTableData: StockDistributionRowDetailsTableData[]=[];

  selectedView : viewType = "default"

  ptableSummery:object;
  
  permissionGroup: PermissionGroup = new PermissionGroup();
  private initPermissionGroup() {
      this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
  }

  constructor(private modalService: NgbModal,
    private fileUtilityService:FileUtilityService,
    private inventoryManagementService: InventoryManagementService,
    private alertService: AlertService,
    private activatedRoute: ActivatedRoute,
    private activityPermissionService: ActivityPermissionService) {
      this.initPermissionGroup()
    }

  ngOnInit() {
    this.getTransactions();
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
    return Utility.getPaginationStatus(this.total, this.pageIndex, this.pageSize,this.stockReceivedTableData.length);
  }


  getTransactions(){
    this.inventoryManagementService.getStockReceiveTransactions(this.pageIndex,this.pageSize,this.search || "",this.fromDate,this.toDate).subscribe(res=>{
      this.transactions = res.data;
      this.total = res.count;
      this.mapTransationsToTableData();
      this.paging.updatePaginatorState();
    });
  }

  mapTransationsToTableData(){
    this.stockReceivedTableData = this.transactions.map(x=>{
      const data = new StockReceiveTableData();
      data.transactionId = x.id;
      data.centralWareHouseName = x.wareHouseModel? x.wareHouseModel.name:"";
      data.salesPointName = x.salesPoint.name;
      data.transactionNumber = x.transactionNumber;
      if(x.referenceTransaction) data.chalanNumber = x.referenceTransaction.transactionNumber;
      data.date  = Utility.getDateToStringFormat(x.transactionDateStr);
      data.quantity = x.wDistributionRecieveTransactions.reduce((total,item)=> total+item.recievedQuantity,0);
      data.lines = x.wDistributionRecieveTransactions.length;
      data.isConfirmed = x.isConfirmed;
      data.disableCheck = !!x.isConfirmed;
      if(x.transactionStatus === TransactionStatus.Pending && !!x.isConfirmed) data.transactionStatus = "Confirmed";
      else data.transactionStatus =   TransactionStatusStrs[x.transactionStatus];
      return data;
    })
  }

  getTransactionDate(dateStr:string){
    return  Utility.getDateToStringFormat(dateStr);
  }

  mapSelectedTransactionToDetailsTableData(){
    let data:StockDistributionRowDetailsTableData[]=[];
    data = this.selectedTransaction.wDistributionRecieveTransactions.map(x=>{
      const item = new StockDistributionRowDetailsTableData();
      if(x.posmProduct) item.posmName = x.posmProduct.name;
      item.quantity = x.quantity;
      item.receivedQuantity = x.recievedQuantity;
      return item;
    });
    this.stockDistributionRowDetailsTableData = data;
    
  }

  fnTransactionPtableCellClick(event){
    if(event.cellName !=="details") return;
    console.log(event);
    let row:StockDistributionTableData = event.record;
    this.selectedTransaction = this.transactions.find(x=>x.id === row.transactionId);
    if(!this.selectedTransaction) return;
    this.mapSelectedTransactionToDetailsTableData();
    this.selectedView = "transactionDetails";
  }

  paginate(event) {
    this.pageIndex = Number(event.page) + 1;
    this.getTransactions();
    this.transactionPtableSettings.serverSitePageIndex = this.pageIndex;
}

haneleCustomActivityOnRecord({ action: action, record: recordInfo }){
  console.log(action,recordInfo);
  if(action === "view-item"){
    this.selectedTransaction = this.transactions.find(x=>recordInfo.transactionId === x.id);
    this.mapSelectedTransactionToDetailsTableData();
    this.selectedView = "transactionDetails";
    this.ptableSummery = {
      "Central ware house": this.selectedTransaction.wareHouseModel.name,
      "Salespoint":this.selectedTransaction.salesPoint.name,
      "Transaction Id":this.selectedTransaction.transactionNumber,
      "Transaction Date":this.selectedTransaction.transactionDateStr,
    }
  }
  if(action === "check-item"){
    let selectedTransaction = this.transactions.find(x=>recordInfo.transactionId === x.id);
    this.alertService.confirm("Are you sure you want to confirm this item?", () => {
      this.inventoryManagementService.confirmStockDistributionTransaction(selectedTransaction.id).subscribe(data=>{
        selectedTransaction.isConfirmed = true;
        this.mapTransationsToTableData();
        this.alertService.tosterSuccess("Successfully confirmed.");
      });

      }, () => {

      });
    
  }
}

setViewView(viewType:viewType){
  this.selectedView = viewType;
  if(viewType === "default") {
    this.getTransactions();
  }
}

reset(){
  this.toDate = new Date().toISOString();
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

handleDateRange(dateRange:IDateRange){
  Utility.adjustDateRange(dateRange);
  this.fromDate = dateRange.from;
  this.toDate = dateRange.to;
  this.paging.changePage(0);
}


}
