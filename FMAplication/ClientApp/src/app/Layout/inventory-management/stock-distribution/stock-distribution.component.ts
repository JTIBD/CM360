import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Paginator } from 'primeng/paginator';
import { CreatePOSM_Distribution, StockDistributionRowDetailsTableData, StockDistributionTableData, WDistributionTransactionProduct } from 'src/app/Shared/Entity';
import { Transaction } from 'src/app/Shared/Entity/Inventory/Transaction';
import { TransactionStatus, TransactionStatusStrs } from 'src/app/Shared/Enums/TransactionStatus';
import { IDateRange, IParsedExcel, IParseExcelModel } from 'src/app/Shared/interfaces';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { FileUtilityService } from 'src/app/Shared/Services/fileUtility/file-utility.service';
import { Utility } from 'src/app/Shared/utility';
import { DownloadStockDistributionModalComponent } from '../download-stock-distribution-modal/download-stock-distribution-modal.component';
import { InventoryManagementService } from '../inventory-management.service';

type viewType =  "default"|"transactionDetails";

@Component({
  selector: 'app-stock-distribution',
  templateUrl: './stock-distribution.component.html',
  styleUrls: ['./stock-distribution.component.css']
})
export class StockDistributionComponent implements OnInit {

  excelData:IParsedExcel;
  isEditing = false;
  remark: string;
  selectedDate: string = Utility.getDateToStringFormat(new Date().toISOString()) ;
  transactions: Transaction[];
  stockDistributionTableData: StockDistributionTableData[]=[];
  stockDistributionRowDetailsTableData: StockDistributionRowDetailsTableData[]=[];
  selectedTransaction:Transaction=null;
  selectedView : viewType = "default"
  ptableSummery:object;

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
    tableName: "Stock Distribution Transactions",
    tableRowIDInternalName: "id",
    tableColDef: [
      { 
        headerName: "Warehouse Name", 
        width: "15%", 
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
        width: "10%", 
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
    //enabledAutoScrolled:true,
    //enabledCellClick: true,
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
    tableName: "Transaction Details",
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
   
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: this.detailsTablePazeSize,
    enabledPagination: true,
    //enabledAutoScrolled:true,
    //enabledCellClick: true,
    enabledColumnFilter: true,
    enabledDeleteBtn: false,
    enabledPrint:true,
    
  };

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
      this.initPermissionGroup();
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
    return Utility.getPaginationStatus(this.total, this.pageIndex, this.pageSize,this.stockDistributionTableData.length);
  }


  openExcelImportModal() {
      let ngbModalOptions: NgbModalOptions = {
          backdrop: 'static',
          keyboard: false,
          size:'lg'
      };
      const modalRef = this.modalService.open(DownloadStockDistributionModalComponent, ngbModalOptions);

      modalRef.result.then((result) => {
          /*this.closeResult = `Closed with: ${result}`;
          this.router.navigate(['/users/users-list/']);*/
      },
          (reason) => {
          });
  }

  handleUpload(e: Event) {
    const target = e.target as HTMLInputElement;
    const file = target.files[0];
    if (file) {
        let model: IParseExcelModel = {
            file
        }
        this.fileUtilityService.ParseExcel(model).subscribe((data) => {
            this.excelData = data;
            this.createTransactions(data);              
        });
    }
    target.value="";
  }

  getTransactionDate(dateStr:string){
    return  Utility.getDateToStringFormat(dateStr);
  }

  mapSelectedTransactionToDetailsTableData(){
    let data:StockDistributionRowDetailsTableData[]=[];
    data = this.selectedTransaction.wDistributionTransactions.map(x=>{
      const item = new StockDistributionRowDetailsTableData();
      item.posmName = x.posmProductModel.name;
      item.quantity = x.quantity;
      return item;
    });
    this.stockDistributionRowDetailsTableData = data;
    
  }

  mapTransationsToTableData(){
    this.stockDistributionTableData = this.transactions.map(x=>{
      const data = new StockDistributionTableData();
      data.transactionId = x.id;
      if(x.wareHouseModel) data.centralWareHouseName =  x.wareHouseModel.name;
      if(x.salesPoint) data.salesPointName = x.salesPoint.name;
      data.transactionNumber = x.transactionNumber;
      data.chalanNumber = x.transactionNumber;
      data.date  = Utility.getDateToStringFormat(x.transactionDateStr);
      data.quantity = x.wDistributionTransactions.reduce((total,item)=> total+item.quantity,0);
      data.lines = x.wDistributionTransactions.length;
      data.isConfirmed = x.isConfirmed;
      data.disableCheck = !!x.isConfirmed;
      if(x.transactionStatus === TransactionStatus.Pending && !!x.isConfirmed) data.transactionStatus = "Confirmed";
      else data.transactionStatus =   TransactionStatusStrs[x.transactionStatus];
      return data;
    })
  }

  createTransactions(excelData:IParsedExcel){
    let payload = new CreatePOSM_Distribution();
    let wDistributionTransactionProducts:WDistributionTransactionProduct[]=[];
    payload.wareHouseCode = excelData.rows[1][0];
    const posmNames:string[] = excelData.rows[0].slice(3);
    excelData.rows.slice(1).forEach(r=>{
      let salesPointId = r[1];
      let quantities = r.slice(3);
      quantities.forEach((q,index)=>{
        let wDistributionTransactionProduct = new WDistributionTransactionProduct();
        wDistributionTransactionProduct.salesPointCode = salesPointId;
        wDistributionTransactionProduct.posM_Name = posmNames[index];
        wDistributionTransactionProduct.quantity = q;
        wDistributionTransactionProducts.push(wDistributionTransactionProduct);
      })      
    })
    
    payload.wDistributionTransactionProducts = wDistributionTransactionProducts.filter(x=>!!x.quantity);

    this.validteQuantities(payload.wDistributionTransactionProducts);

    if(!payload.wDistributionTransactionProducts.length) {
      this.alertService.tosterDanger("No products found to distribute");
      return;
    }

    this.inventoryManagementService.addWPOSM_DistributionTransaction(payload).subscribe(data=>{
      // this.transactions = [...data,...this.transactions];
      this.reset()
      // this.getTransactions();
      this.alertService.tosterInfo("Successfully created transaction");
    },(err)=>{
    });
  }

  validteQuantities(items:WDistributionTransactionProduct[]){
    let negativeValueItems = items.filter(x=>x.quantity < 0);
    if(negativeValueItems.length){
      let productNames = negativeValueItems.map(x=>x.posM_Name).join(", ");
      this.alertService.tosterDanger(`Negative quantity found for product ${productNames}`);
      throw "Negative quantity found";
    }
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

  getTransactions(){
    this.inventoryManagementService.getStockDistriTransactions(this.pageIndex,this.pageSize,this.search || "",this.fromDate,this.toDate).subscribe(data=>{
      this.transactions = data.data;
      this.total = data.count;
      this.mapTransationsToTableData();
      this.paging.updatePaginatorState();
    });
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
      },(err)=>{
        console.log(err.error);
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
