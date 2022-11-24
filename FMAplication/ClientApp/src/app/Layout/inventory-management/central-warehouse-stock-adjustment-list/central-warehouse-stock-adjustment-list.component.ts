import { IDropdown } from './../../../Shared/interfaces/IDropdown';
import { AlertService } from './../../../Shared/Modules/alert/alert.service';
import { AdjustmentTransactionParams } from './../../../Shared/Entity/Inventory/StockAdjustmentTransaction';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Paginator } from 'primeng/paginator';
import { IStockAdjustmentTransaction, StockAdjustmentTableData } from 'src/app/Shared/Entity/Inventory';
import { TransactionStatus } from 'src/app/Shared/Enums/TransactionStatus';
import { IDateRange, IPaginator } from 'src/app/Shared/interfaces';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { Utility } from 'src/app/Shared/utility';
import { InventoryManagementService } from '../inventory-management.service';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { TWStatus } from 'src/app/Shared/Enums/TWStatus';

@Component({
  selector: 'app-central-warehouse-stock-adjustment-list',
  templateUrl: './central-warehouse-stock-adjustment-list.component.html',
  styleUrls: ['./central-warehouse-stock-adjustment-list.component.css']
})
export class CentralWarehouseStockAdjustmentListComponent implements OnInit {


  dropdownData : IDropdown[] = [{label:'All', value:-1},{label:'Pending', value:0}, {label:"Waiting for approval", value:1}, {label:"Completed", value:2}];
  totalNumber: number;
  transactions: IStockAdjustmentTransaction[];
  stockAdjustmentTableData: StockAdjustmentTableData[]=[];
  params:AdjustmentTransactionParams = new AdjustmentTransactionParams();

  public transactionPtableSettings: IPTableSetting = {
    tableID: "transactionPtable",
    tableClass: "table table-border ",
    tableName: "",
    tableRowIDInternalName: "id",
    tableColDef: [
      {
        headerName: "Transaction Number",
        width: "15%",
        internalName: "transactionNumber",
        sort: true,
        type: "",
      },
      {
        headerName: "Transaction Date",
        width: "10%",
        internalName: "transactionDate",
        sort: true,
        type: "",
      },
      {
        headerName: "#Lines ",
        width: "5%",
        internalName: "lines",
        sort: true,
        type: "",
      },
      {
        headerName: "Total Increase ",
        width: "15%",
        internalName: "totalIncrease",
        sort: true,
        type: "",
      },
      {
        headerName: "Total Decrease ",
        width: "15%",
        internalName: "totalDecrease",
        sort: true,
        type: "",
      },
      {
        headerName: "Status ",
        width: "15%",
        internalName: "status",
        sort: true,
        type: "",
      },
      {
        headerName: "Remark ",
        width: "25%",
        internalName: "remarks",
        sort: true,
        type: "",
      }
    ],

    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: this.params.pageSize,
    enabledPagination: true,
    //enabledAutoScrolled:true,
    //enabledCellClick: true,
    enabledColumnFilter: true,
    enabledDeleteBtn: false,
    tableFooterVisibility: false,
    enabledViewBtn: true,
    enabledCheckBtn: true,
    enabledEditBtn: true,
    enabledServerSitePaggination: true,
    enablePazeSizeSelection: true,
    enabledDataLength: true, 
    enableDropdownFilter:true,
    dropdownData:  this.dropdownData, 
    selectedDropdownValue : 0, 
    dropdownLabel: "Status", 
    enableDateRangeFilter:true,
    intialDateRange:{
      from:this.params.fromDate,
      to:this.params.toDate,
    },


  };

  @ViewChild("paging", { static: false }) paging: Paginator;
  // showingPageDetails:any;

  permissionGroup: PermissionGroup = new PermissionGroup();
  private initPermissionGroup() {
      this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
      this.transactionPtableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
  }
  constructor(private inventoryManagementService: InventoryManagementService, private alert: AlertService, private router: Router,
    private activatedRoute: ActivatedRoute, private activityPermissionService: ActivityPermissionService) {
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
 return  Utility.getPaginationStatus(this.totalNumber,this.params.pageIndex,this.params.pageSize,this.stockAdjustmentTableData.length); 
}


  getTransactions() {
    this.inventoryManagementService.getStockAdjustmentTransactions(this.params).subscribe((data: IPaginator<IStockAdjustmentTransaction>) => {
      this.transactions = []
      this.stockAdjustmentTableData = [];
      this.transactions = data.data;
      this.totalNumber = data.count;
      this.paging.totalRecords = this.totalNumber;
      this.transactions.forEach(transaction => {
        transaction.transactionDate = Utility.getDateToStringFormat(transaction.transactionDate);
        this.mapTransationsToTableData();
      });
    })
  }
  mapTransationsToTableData() {

    this.stockAdjustmentTableData = this.transactions.map(x => {
      const data = new StockAdjustmentTableData();
      data.id = x.id;
      data.transactionNumber = x.transactionNumber;
      data.transactionDate = x.transactionDate;
      data.totalIncrease = x.totalIncrease;
      data.totalDecrease = x.totalDecrease;
      data.lines = x.lines;
      data.remarks = x.remarks;
      data.isConfirmed = x.isConfirmed;
      data.disableCheck = !!x.isConfirmed;
      data.disableView = !x.isConfirmed;
      data.disableEdit = x.isConfirmed
      data.status = this.getTransactionStatus(x);
      return data;
    });
  }
  paginate(event) {
    this.params.pageIndex = Number(event.page) + 1;
    this.params.pageSize = event.rows;
    this.getTransactions();
    this.transactionPtableSettings.serverSitePageIndex = this.params.pageIndex;
  }


  addNewStockAdjust() {
    this.router.navigate(['/inventory/stock-adjustment']);
  }

  getTransactionStatus(t: IStockAdjustmentTransaction) {
    let result = "";
    if (t.transactionStatus === TransactionStatus.Pending && !t.isConfirmed)
      result = "Pending";
    else if (t.transactionStatus === TransactionStatus.Pending && t.isConfirmed) {
      result = "Waiting for approval";
    }
    else if (t.transactionStatus === TransactionStatus.WaitingForApproval) {
      result = this.getWorkFlowStatus(t);
    }
    else if (t.transactionStatus === TransactionStatus.Completed) {
      result = "Completed";
    }

    return result;
  }

  getWorkFlowStatus(t: IStockAdjustmentTransaction): string {
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

  confirmTransaction(id: number) {
    this.alert.confirm("Are you sure you want to Confirm this transaction ?", () => {
      this.inventoryManagementService.confirmStockAdjustmentTransaction(id).subscribe(
        (res: any) => {
          console.log('res from del func', res);
          this.alert.tosterSuccess("Transaction has been confirmed");
          this.getTransactions();
        },
        (error) => {
          console.log(error);
        }
      );
    }, 
    () => {

    });
  }


  haneleCustomActivityOnRecord({ action: action, record: recordInfo }) {
    let id = recordInfo.id;
    if (action === "view-item") {
      this.router.navigate(['/inventory/stock-adjustment', id]);
    }
    if (action === "check-item") {
      this.confirmTransaction(id);
    }
    if (action === "edit-item") {
      this.router.navigate(['/inventory/stock-adjustment', id]);
    }
  }

  fnTransactionPtableCellClick(event) {
    if (event.cellName !== "details") return;
  }

  fnSearch($event: any) {
    this.params.search = $event.searchVal;
    this.paging.changePage(0);
  }

  handlePazeSizeChange(pageSize: number) {
    this.params.pageSize = +pageSize;
    this.getTransactions(); 
    this.paging.changePageToFirst;

  }
  
  dropDownChange(selected:string){
    this.params.transactionStatus = Number.parseInt(selected);
    this.getTransactions();
    this.paging.changePageToFirst;
  }

  handleDateRange(dateRange:IDateRange){
    this.params.fromDate = dateRange.from;
    this.params.toDate = dateRange.to;
    this.paging.changePage(0);
  
  }


}


