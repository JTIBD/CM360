import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Paginator } from 'primeng/paginator';
import { StockAddTransaction } from 'src/app/Shared/Entity/Inventory/StockAddTransaction';
import { TransactionStatus, TransactionStatusStrs } from 'src/app/Shared/Enums/TransactionStatus';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { Utility } from 'src/app/Shared/utility';
import { Stock, StockAddDetailsTableData, StockAddTransactionTableData } from '../../../Shared/Entity';
import { CreateStockAddTransaction, POSM_Product_Quantity, UpdateStockAddTransaction } from '../../../Shared/Entity/Inventory';
import { Transaction } from '../../../Shared/Entity/Inventory/Transaction';
import { IDateRange, IParsedExcel, IParseExcelModel } from '../../../Shared/interfaces';
import { IPTableSetting } from '../../../Shared/Modules/p-table';
import { CommonService } from '../../../Shared/Services/Common/common.service';
import { FileUtilityService } from '../../../Shared/Services/fileUtility/file-utility.service';
import { DownloadStockModalComponent } from '../download-stock-modal/download-stock-modal.component';
import { InventoryManagementService } from '../inventory-management.service';

type viewType =  "default"|"transactionDetails"|"transactionEdit";
@Component({
  selector: 'app-central-warehouse-stock-add',
  templateUrl: './central-warehouse-stock-add.component.html',
  styleUrls: ['./central-warehouse-stock-add.component.css']
})
export class CentralWarehouseStockAddComponent implements OnInit {

    private excelData: IParsedExcel;
    remark: string;
    selectedDate: string = Utility.getDateToStringFormat(new Date().toISOString()) ;
    transactions: Transaction[];
    selectedTransaction:Transaction;
    viewMode: "Transactions" | "UploadedStocks" = "Transactions"
    isEditing = false;
    transactionTableData:StockAddTransactionTableData[]=[];
    transactionDetailsTableData:StockAddDetailsTableData[]=[];
    selectedView : viewType = "default";
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
    tableName: "Stock Add Transactions",
    tableRowIDInternalName: "id",
    tableColDef: [
      { 
        headerName: "Warehouse Name", 
        width: "20%", 
        internalName: "centralWareHouseName",
        sort: true, 
        type: "",
       },

      { 
        headerName: "Transaction Number", 
        width: "20%", 
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
        headerName: "Remarks", 
        width: "15%", 
        internalName: "remarks", 
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
    enabledEditBtn:true,
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

  transactionDetailsViewPtableSettings:IPTableSetting = {
    tableID: "transactionDetailsPtable",
    tableClass: "table table-border ",
    tableName: "Transaction Details",
    tableRowIDInternalName: "id",
    tableColDef: [
      { 
        headerName: "Central ware house", 
        width: "20%", 
        internalName: "wareHouse",
        sort: true, 
        type: "",
       },
      { 
        headerName: "POSM Code", 
        width: "15%", 
        internalName: "skuCode", 
        sort: true, 
        type: "",
       },
       { 
        headerName: "POSM Name", 
        width: "20%", 
        internalName: "skuName", 
        sort: true, 
        type: "",
       },
       { 
        headerName: "Quantity", 
        width: "15%", 
        internalName: "quantity", 
        sort: true, 
        type: "",
       },
       {
        headerName: "Supplier",
        width: "20%",
        internalName: "supplier",
        sort: true,
        type: ""
       }
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: this.detailsTablePazeSize,
    enabledPagination: true,
    //enabledAutoScrolled:true,
    //enabledCellClick: true,
    enabledColumnFilter: true,
    enabledDeleteBtn: false,
    enabledPrint:false,
    
  };

  public transactionDetailsPtableSettings: IPTableSetting= this.transactionDetailsViewPtableSettings;

  permissionGroup: PermissionGroup = new PermissionGroup();
  private initPermissionGroup() {
      this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
  }

  constructor(private inventoryManagementService: InventoryManagementService,
      private commonService: CommonService,
      private fileUtilityService: FileUtilityService,
      private modalService: NgbModal,
      private alertService: AlertService,
      private activatedRoute: ActivatedRoute,
      private activityPermissionService: ActivityPermissionService
  ) { 
    this.initPermissionGroup();
  }

  ngOnInit() {
      this.getTransactions();
  }

  ngAfterViewInit(){
       this.enableCurrentPageReport();
  }

  enableCurrentPageReport(){
    let timer = setInterval(()=>{
      if(this.paging){
        this.paging.showCurrentPageReport=true;
        clearInterval(timer);
      }
    },1); 
  }

  getPaginationStatus(){    
    return Utility.getPaginationStatus(this.total,this.pageIndex,this.pageSize,this.transactionTableData.length);    
  }


  getTransactions(){
    this.inventoryManagementService.getStockAddTransactions(this.pageIndex,this.pageSize,this.search,this.fromDate,this.toDate).subscribe(res => {
        this.transactions = res.data;
        this.total = res.count;
        this.mapTransationsToTableData();
        this.paging.updatePaginatorState();
    })
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
                // this.stocks = this.excelData.rows.slice(1).map((x, index) => ({
                //     quantity: Number(x[3]),
                //     sequence: index,
                //     skuCode: String(x[1]),
                //     skuName: String(x[2]),
                //     wareHouse: String(x[0]),
                //     url: ""
                // })).filter(x => !!x.quantity);
                this.selectedTransaction=null;
                this.transactionDetailsTableData=this.excelData.rows.slice(1).map((x,index)=>{
                  const rowData = new StockAddDetailsTableData();
                  rowData.id = index;
                  rowData.quantity = Number(x[3]);
                  rowData.skuCode = x[1];
                  rowData.skuName = x[2];
                  rowData.wareHouse=x[0];
                  rowData.supplier = x[4];
                  return rowData;
                }).filter(x => !!x.quantity);
                if(!this.transactionDetailsTableData.length) {
                  this.alertService.tosterDanger("No Products found");
                  return;
                }
                this.setPtableEditSettings();
                this.selectedView = 'transactionEdit';
                //this.isEditing = true;
                this.selectedDate = Utility.getDateToStringFormat(new Date().toISOString());
            });
        }
      target.value="";
    }

    setPtableEditSettings(){
      this.transactionDetailsPtableSettings.tableColDef[3].type="text-field";
      this.transactionDetailsPtableSettings.enabledDeleteBtn = true;
    }
    setPtableViewSettings(){
      this.transactionDetailsPtableSettings.tableColDef[3].type="";
      this.transactionDetailsPtableSettings.enabledDeleteBtn = false;
    }

    mapTransationsToTableData(){
        this.transactionTableData = this.transactions.map(x=>{
          const data = new StockAddTransactionTableData();
          data.transactionId = x.id;
          data.centralWareHouseName = x.wareHouseModel? x.wareHouseModel.name:"";
          data.transactionNumber = x.transactionNumber;
          data.date  = Utility.getDateToStringFormat(x.transactionDateStr);
          data.quantity = x.stockAddTransactions.reduce((total,item)=> total+item.quantity,0);
          data.lines = x.stockAddTransactions.length;
          data.isConfirmed = x.isConfirmed;
          data.disableCheck = !!x.isConfirmed;
          data.disableView = !x.isConfirmed;
          data.disableEdit = !!x.isConfirmed;
          data.remarks = x.remarks;
          if(x.transactionStatus === TransactionStatus.Pending && !!x.isConfirmed) data.transactionStatus = "Confirmed";
          else data.transactionStatus =   TransactionStatusStrs[x.transactionStatus];
          return data;
        })
    }

    mapSelectedTransactionToDetailsTableData(){
        let data:StockAddDetailsTableData[]=[];
        let wareHouse = "";
        this.remark = this.selectedTransaction.remarks;
        if(this.selectedTransaction.wareHouseModel) wareHouse = this.selectedTransaction.wareHouseModel.name;

        data = this.selectedTransaction.stockAddTransactions.map((x,index)=>{
          const item = new StockAddDetailsTableData();
          item.id = index;
          item.skuName = x.posmProductModel.name;
          item.skuCode = x.posmProductModel.code;
          item.quantity = x.quantity;
          item.wareHouse = wareHouse;
          item.supplier = x.supplier;
          // item.supplier = 
          return item;
        });
        this.transactionDetailsTableData = data;
        
      }

    fnTransactionPtableCellClick(event){
        if(event.cellName !=="details") return;
        console.log(event);
        let row:StockAddTransactionTableData = event.record;
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

    getTransactionDate(dateStr:string){
        return  Utility.getDateToStringFormat(dateStr);
    }

    setViewView(viewType:viewType){
        this.selectedView = viewType;
        if(viewType === "default") {
          this.getTransactions();
          // this.paging.showCurrentPageReport = true;
        }
    }

    viewRow(row:StockAddTransactionTableData){
      this.setPtableViewSettings();
      this.selectedTransaction = this.transactions.find(x=>row.transactionId === x.id);
      this.mapSelectedTransactionToDetailsTableData();
      this.selectedView = "transactionDetails";
      this.ptableSummery = {
        "Central ware house": this.selectedTransaction.wareHouseModel.name,
        "Transaction Id":this.selectedTransaction.transactionNumber,
        "Transaction Date":this.selectedTransaction.transactionDateStr,
      }
    }

    handleCheckClick(row:StockAddTransactionTableData){
      let selectedTransaction = this.transactions.find(x=>row.transactionId === x.id);
      this.alertService.confirm("Are you sure you want to confirm this item?", () => {
        this.inventoryManagementService.confirmStockAddTransaction(selectedTransaction.id).subscribe(data=>{
          selectedTransaction.isConfirmed = true;
          this.mapTransationsToTableData();
          this.alertService.tosterSuccess("Successfully confirmed.");
        });
  
        }, () => {
  
        });
    }

    handleEditRow(row:StockAddTransactionTableData){
      let selectedTransaction = this.transactions.find(x=>row.transactionId === x.id);
      this.selectedTransaction = selectedTransaction;
      console.log(selectedTransaction);
      this.setPtableEditSettings();
      this.mapSelectedTransactionToDetailsTableData();
      this.selectedView = "transactionEdit";  
    }

    haneleCustomActivityOnRecord({ action: action, record: recordInfo }){
        console.log(action,recordInfo);
        if(action === "view-item"){
          this.viewRow(recordInfo);
        }
        if(action === "check-item"){
          this.handleCheckClick(recordInfo);          
        }
        if(action === "edit-item"){
          this.handleEditRow(recordInfo);
        }
      }

      haneleDetailsTableCustomActivityOnRecord({ action, record }){
        console.log(record);
        console.log(this.transactionDetailsTableData)
        if (action == "delete-item") {
          this.transactionDetailsTableData = this.transactionDetailsTableData.filter(x=> x.id !== record.id);
        }
      }

      

    isFormValid() {
        if (this.transactionTableData.some(st => isNaN(st.quantity))) return false;
        return true;
    }

    submitForm() {
        if (this.isFormValid()) {
        }
    }


    // handleChange(e: Event,code:string) {
    //     const target = e.target as HTMLInputElement;
    //     const quantity = Number(target.value);
    //     if (isNaN(quantity)) return;
    //     const stock = this.transactionTableData.find(s => s.co === code);
    //     if (stock) stock.quantity = Number(quantity);
    // }

    openExcelImportModal() {
        let ngbModalOptions: NgbModalOptions = {
            backdrop: 'static',
            keyboard: false
        };
        const modalRef = this.modalService.open(DownloadStockModalComponent, ngbModalOptions);

        modalRef.result.then((result) => {
            /*this.closeResult = `Closed with: ${result}`;
            this.router.navigate(['/users/users-list/']);*/
        },
            (reason) => {
            });
    }

    showDefaultView() {
        this.selectedTransaction = null;
        this.selectedView="default";
        this.transactionDetailsPtableSettings= this.transactionDetailsViewPtableSettings;
        this.remark = "";
    }

    saveTransactions(){
        if(!this.selectedTransaction) this.createTransactions();
        else {
               this.updateTransactions();
        }
    }

    updateTransactions(){
        if(!this.transactionDetailsTableData.length) return;
        
        let stockAddTransactions = this.selectedTransaction.stockAddTransactions;
        stockAddTransactions = stockAddTransactions.filter(x=> this.transactionDetailsTableData.some(s=>s.skuCode === x.posmProductModel.code));
        for(let x of stockAddTransactions){
            x.quantity = this.transactionDetailsTableData.find(st=>st.skuCode === x.posmProductModel.code).quantity
            if(isNaN(x.quantity)) {
              this.alertService.tosterDanger(`${x.quantity} is not a valid number`);
              return;
            }
        };
        let updateTransaction = new UpdateStockAddTransaction();
        updateTransaction.remarks = this.remark;
        updateTransaction.stockAddTransactions = stockAddTransactions;
        this.inventoryManagementService.updateStockAddTransaction(updateTransaction).subscribe(res=>{
            this.selectedTransaction.stockAddTransactions = stockAddTransactions;
            this.selectedTransaction.remarks = this.remark;
            this.mapTransationsToTableData();
            this.showDefaultView();
        },(err)=>{
            this.showDefaultView();
        });
    }

    createTransactions() {
        let payload: CreateStockAddTransaction = new CreateStockAddTransaction();
        payload.remarks = this.remark;
        payload.wareHouseCode = this.transactionDetailsTableData[0].wareHouse.split("(")[1].split(")")[0];
        let posM_Products: POSM_Product_Quantity[] = [];
        for(let st of this.transactionDetailsTableData) {
            const p: POSM_Product_Quantity = new POSM_Product_Quantity();
            p.posmProductCode = st.skuCode;
            if(isNaN(st.quantity)){
              this.alertService.tosterDanger(`${st.quantity} is not a valid quantity`);
              return;
            }
            p.quantity = st.quantity;
            p.supplier = st.supplier;
            posM_Products.push(p);
        };
        payload.posM_Products = posM_Products;
        this.inventoryManagementService.createAddStockTransaction(payload).subscribe(data => {
          this.selectedView="default";
          this.transactionDetailsTableData = [];
          this.reset();
          this.alertService.tosterInfo("Successfully created transaction");
        });
    }

    getStockAddDateString(date:string = new Date().toISOString()){
        console.log(date);
        //let dateObj = new Date(date);
        return Utility.getDateToStringFormat(date)
    }

    getStockAddQuantity(tr:Transaction){
        let quantity = 0;
        tr.stockAddTransactions.forEach(addTr=>{
            quantity += addTr.quantity;
        })
        return quantity;
    }
    
    confirmTransaction(transaction:Transaction){
        transaction.isConfirmed = true;
        this.inventoryManagementService.confirmStockAddTransaction(transaction.id).subscribe(data=>{
            
        },(err)=>{
            transaction.isConfirmed = false;
        })
    }
    // handleEditTransaction(transaction:Transaction){
    //     this.isEditing=true;
    //     this.selectedTransaction = transaction;
    //     this.remark = transaction.remarks;
    //     this.selectedDate = Utility.getDateToStringFormat(transaction.transactionDate);
    //     this.stocks = transaction.stockAddTransactions.map(x=> this.getStockFromAddTransaction(x,transaction));        
    // }
    // removeStock(stock:Stock){
    //     this.stocks = this.stocks.filter(x=>x.skuCode !== stock.skuCode);
    // }

    getStockFromAddTransaction(stockAddTransaction:StockAddTransaction,transaction: Transaction){
        const stock = new Stock();
        stock.quantity = stockAddTransaction.quantity;
        stock.skuCode = stockAddTransaction.posmProductModel.code;
        stock.skuName = stockAddTransaction.posmProductModel.name;
        stock.wareHouse = `${transaction.wareHouseModel.name}(${transaction.wareHouseModel.code})`;
        return stock;
    }

    // showTransactionDetails(transaction:Transaction){        
    //     this.selectedTransaction = transaction;
    //     this.isEditing = false;
    //     this.stocks = transaction.stockAddTransactions.map(x => this.getStockFromAddTransaction(x,transaction))
    // }
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
