import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Paginator } from 'primeng/paginator';
import { SalesPointTransfer, PosmProduct, SalesPoint, SalesPointTransferItem, SalesPointTransferDetailsTableData, SalesPointTransferTableData } from 'src/app/Shared/Entity';
import { TransactionStatus, TransactionStatusStrs } from 'src/app/Shared/Enums/TransactionStatus';
import { TWStatus } from 'src/app/Shared/Enums/TWStatus';
import { IParsedExcel, IParseExcelModel, IDateRange } from 'src/app/Shared/interfaces';
import { IDropdown } from 'src/app/Shared/interfaces/IDropdown';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { colDef, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { RoutesInventoryManagement } from 'src/app/Shared/Routes/RoutesInventoryManagement';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { FileUtilityService } from 'src/app/Shared/Services/fileUtility/file-utility.service';
import { PosmProductService } from 'src/app/Shared/Services/Product/posmproduct.service';
import { SalesPointService } from 'src/app/Shared/Services/Sales';
import { Utility } from 'src/app/Shared/utility';
import { InventoryManagementService } from '../inventory-management.service';
import { ModalDownloadSpTransferFormatComponent } from '../modal-download-sp-transfer-format/modal-download-sp-transfer-format.component';

type viewType =  "default"|"transactionDetails";

@Component({
  selector: 'app-salespoint-transfer',
  templateUrl: './salespoint-transfer.component.html',
  styleUrls: ['./salespoint-transfer.component.css']
})
export class SalespointTransferComponent implements OnInit {

  posmProducts:PosmProduct[]=[];
  salesPoints:SalesPoint[]=[];
  excelData:IParsedExcel;
  isEditing = false;
  remark: string;
  selectedDate: string = Utility.getDateToStringFormat(new Date().toISOString()) ;
  transactions: SalesPointTransfer[];
  stockDistributionTableData: SalesPointTransferTableData[]=[];
  stockDistributionRowDetailsTableData: SalesPointTransferDetailsTableData[]=[];
  selectedTransaction:SalesPointTransfer=null;
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

  dropdownData : IDropdown[] = [{label:'All', value:-1},{label:'Pending', value:0}, {label:"Waiting for approval", value:1}, {label:"Completed", value:2}];

  public transactionPtableSettings: IPTableSetting<colDef<keyof SalesPointTransferTableData>> = {
    tableID: "transactionPtable",
    tableClass: "table table-border ",
    tableName: "Salespoint Transfers",
    tableRowIDInternalName: "id",
    tableColDef: [
      { 
        headerName: "Source Salespoint", 
        width: "15%", 
        internalName: "sourceSalesPointName",
        sort: true, 
        type: "",
       },
      { 
        headerName: "Destination Salespoint", 
        width: "15%", 
        internalName: "destinationSalesPointName", 
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
        width: "15%", 
        internalName: "date", 
        sort: true, 
        type: "",      
       },
       { 
        headerName: "Status ", 
        width: "15%", 
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
    enableDropdownFilter:true,
    dropdownData:  this.dropdownData, 
    selectedDropdownValue : 0, 
    dropdownLabel: "Status", 
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
  transactionStatus: TransactionStatus | -1 = TransactionStatus.Pending;
  private initPermissionGroup() {
      this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
  }

  constructor(private modalService: NgbModal,
    private fileUtilityService:FileUtilityService,
    private inventoryManagementService: InventoryManagementService,
    private alertService: AlertService,
    private posmProductService: PosmProductService,
    private salesPointService: SalesPointService,
    private activatedRoute: ActivatedRoute, 
    private activityPermissionService: ActivityPermissionService,
    private router: Router) {

      this.router.routeReuseStrategy.shouldReuseRoute = () => false;
      this.initPermissionGroup();
      
    }

  ngOnInit() {
    this.getTransactions();
    this.getLazyData();
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
      const modalRef = this.modalService.open(ModalDownloadSpTransferFormatComponent, ngbModalOptions);

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
            this.createTransfers(data);              
        });
    }
    target.value="";
  }

  getTransactionDate(dateStr:string){
    return  Utility.getDateToStringFormat(dateStr);
  }

  mapSelectedTransactionToDetailsTableData(){
    let data:SalesPointTransferDetailsTableData[]=[];
    data = this.selectedTransaction.items.map(x=>{
      const item = new SalesPointTransferDetailsTableData();
      if(x.posmProduct){
        item.posmName = x.posmProduct.name;
      } 
      item.quantity = x.quantity;
      return item;
    });
    this.stockDistributionRowDetailsTableData = data;
    
  }

  mapTransationsToTableData(){
    this.stockDistributionTableData = this.transactions.map(x=>{
      const data = new SalesPointTransferTableData();
      data.id = x.id;
      if(x.fromSalesPoint){
         data.sourceSalesPointName = x.fromSalesPoint.name;
      }
      if(x.toSalesPoint){
         data.destinationSalesPointName = x.toSalesPoint.name;
      }
      data.transactionNumber = x.transactionNumber;
      data.date  = Utility.getDateToStringFormat(x.transactionDateStr);
      data.quantity = x.items.reduce((total,item)=> total+item.quantity,0);
      data.lines = x.items.length;
      data.isConfirmed = x.isConfirmed;
      data.disableCheck = !!x.isConfirmed;
      data.transactionStatus =   this.getTransactionStatus(x);
      return data;
    })
  }

  getTransactionStatus(t: SalesPointTransfer) {
    let result = "";
    if (t.transactionStatus === TransactionStatus.Pending && !t.isConfirmed)
      result = "Pending";
    else if (t.transactionStatus === TransactionStatus.WaitingForReceive)
      result = "Waiting For Receive";
    
    else if (t.transactionStatus === TransactionStatus.WaitingForApproval) {
      result = this.getWorkFlowStatus(t);
    }
    else if (t.transactionStatus === TransactionStatus.Completed) {
      result = "Completed";
    }
    return result;
  }

  getWorkFlowStatus(t: SalesPointTransfer): string {
    let result = 'Waiting for approval'
    if (t.transactionWorkflow  == null)
        return result;
    else if (t.transactionWorkflow && t.transactionWorkflow.user) {
      let {user} = t.transactionWorkflow;
      if (t.transactionWorkflow.twStatus ===  TWStatus.Pending ) {
        result = `Waiting for ${user.name} approval`;
        return result;
      }
      else if (t.transactionWorkflow.twStatus === TWStatus.Rejected){
        result = `Rejected  by ${user.name}`;
        return result;
      }
    }
    else if (t.transactionWorkflow && t.transactionWorkflow.role){
      let {role} = t.transactionWorkflow;
      if (t.transactionWorkflow.twStatus === TWStatus.Pending ) {
        result = `Waiting for ${role.name} approval`;
        return result;
      }
      else if (t.transactionWorkflow.twStatus === TWStatus.Rejected){
        result = `Rejected  by ${role.name}`;
        return result;
      }
    }

  }

  createTransfers(excelData:IParsedExcel){
    let transfers:SalesPointTransfer[] = [];

    let rows = excelData.rows.slice(1);
    let sourceSp = this.salesPoints.find(x=> x.code === rows[0][0]);
    if(!sourceSp) this.alertService.tosterDanger(`Salespoint doesnot exist for code ${rows[0][0]}`)
    const posmNames:string[] = excelData.rows[0].slice(4);

    for(let r of rows){
      let transfer  = new SalesPointTransfer();
      transfer.fromSalesPointId = sourceSp.id;
      let destinationSp = this.salesPoints.find(x=>x.code === r[2]);
      if(!destinationSp) {
        this.alertService.tosterDanger(`Salespoint doesnot exist for code ${r[2]}`);
        return;
      }
      else if(destinationSp.salesPointId === sourceSp.salesPointId){
        this.alertService.tosterDanger(`${destinationSp.name} can not be destination salespoint`);
        return;
      }
      transfer.toSalesPointId = destinationSp.salesPointId;
      let quantities = r.slice(4);
      for(let i = 0;i<quantities.length; i++){
        
        let quantity = Number(quantities[i]);
        if(!quantity) continue;

        let item = new SalesPointTransferItem();
        if(!Utility.isInt(quantity)){
          this.alertService.tosterDanger(`${quantity} is not a valid quantity`);
          return;
        }
        item.quantity = Number(quantity);
        let product = this.posmProducts.find(x=>x.name === posmNames[i]);
        if(!product) {
          this.alertService.tosterDanger(`${posmNames[i]} does not exist`);
          return;
        }
        item.posmProductId = product.id;
        
        transfer.items.push(item);
      }
      if(!!transfer.items.length) transfers.push(transfer);
    }
    this.validateQuantity(transfers)
    this.salesPointService.createTransfers(transfers).subscribe(res=>{
      this.toDate = new Date().toISOString();
      this.getTransactions();
      this.alertService.tosterSuccess(`Transfers created successfully`);
    })   
  }

  validateQuantity(transactions:SalesPointTransfer[]){
    //@ts-ignore
    let items:SalesPointTransferItem[] = transactions.map(x=>x.items).flat();
    let negativeValueItems = items.filter(x=> x.quantity < 0);
    if(negativeValueItems.length){
      let posms = this.posmProducts.filter(x=> negativeValueItems.some(i=>i.posmProductId == x.id)).map(x=>x.name);
      this.alertService.tosterDanger(`Negative quantity found for product ${posms.join(", ")}`);
      throw "negative quantity";
    }
  }

  fnTransactionPtableCellClick(event){
    if(event.cellName !=="details") return;
    let row:SalesPointTransferTableData = event.record;
    this.selectedTransaction = this.transactions.find(x=>x.id === row.id);
    if(!this.selectedTransaction) return;
    this.mapSelectedTransactionToDetailsTableData();
    this.selectedView = "transactionDetails";
  }

  getTransactions(){
    this.salesPointService.getTransfers(this.pageIndex,this.pageSize,this.search || "",this.fromDate,this.toDate,this.transactionStatus).subscribe(res=>{
      this.transactions = res.data;
      this.total = res.count;
      this.mapTransationsToTableData();
      this.paging.updatePaginatorState();
    });
  }

  async getPosmProducts(){
    try{
      const response = await this.posmProductService.getPosmProductList().toPromise();
      this.posmProducts= response.data.model;
    }catch(e){
      console.log(e);
    }
  }

  
  async getSalesPoints(){
    try{
      const response = await this.salesPointService.getAllSalesPoint().toPromise();
      this.salesPoints = response.data;
    }catch(e){
      console.log(e);
    }
  }


  async getLazyData(){
    await this.getPosmProducts();
    await this.getSalesPoints();
  }

  paginate(event) {
    this.pageIndex = Number(event.page) + 1;
    this.getTransactions();    
    this.transactionPtableSettings.serverSitePageIndex = this.pageIndex;
  }

haneleCustomActivityOnRecord({ action, record }:{action,record:SalesPointTransferTableData}){
  console.log(action,record);
  if(action === "view-item"){
    var selectedTransaction = this.transactions.find(x=>record.id === x.id);

    this.router.navigate([RoutesInventoryManagement.Parent,RoutesInventoryManagement.SalesPointTransferDetails,selectedTransaction.id])
    // this.selectedTransaction = this.transactions.find(x=>record.id === x.id);
    // this.mapSelectedTransactionToDetailsTableData();
    // this.selectedView = "transactionDetails";
    // this.ptableSummery = {
    //   "Source Salespoint": this.selectedTransaction.fromSalesPoint.name,
    //   "Destination Salespoint":this.selectedTransaction.toSalesPoint.name,
    //   "Transaction Id":this.selectedTransaction.transactionNumber,
    //   "Transaction Date":this.selectedTransaction.transactionDateStr,
    // }
  }
  if(action === "check-item"){
    let selectedTransaction = this.transactions.find(x=>record.id === x.id);
    this.alertService.confirm("Are you sure you want to confirm this item?", () => {
      this.salesPointService.confirmTransfer(selectedTransaction.id).subscribe(data=>{
        selectedTransaction.isConfirmed = true;
        selectedTransaction.transactionStatus = TransactionStatus.WaitingForApproval;
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

dropDownChange(selected:number){
  this.transactionStatus = selected
  this.paging.changePage(0);
}

}
