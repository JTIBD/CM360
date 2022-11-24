import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Paginator } from 'primeng/paginator';
import { StockAdjustmentTableData, IStockAdjustmentTransaction } from 'src/app/Shared/Entity/Inventory';
import { Transaction } from 'src/app/Shared/Entity/Inventory/Transaction';
import { TransactionStatus, TransactionStatusStrs } from 'src/app/Shared/Enums/TransactionStatus';
import { TWStatus } from 'src/app/Shared/Enums/TWStatus';
import { IDateRange } from 'src/app/Shared/interfaces';
import { IDropdown } from 'src/app/Shared/interfaces/IDropdown';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { colDef, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { RoutesInventoryManagement } from 'src/app/Shared/Routes/RoutesInventoryManagement';
import { RoutesLaout } from 'src/app/Shared/Routes/RoutesLaout';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { Utility } from 'src/app/Shared/utility';
import { InventoryManagementService } from '../inventory-management.service';

@Component({
  selector: 'app-salespoint-stock-adjustment-list',
  templateUrl: './salespoint-stock-adjustment-list.component.html',
  styleUrls: ['./salespoint-stock-adjustment-list.component.css']
})
export class SalespointStockAdjustmentListComponent implements OnInit {

  dropdownData : IDropdown[] = [{label:'All', value:-1},{label:'Pending', value:0}, {label:"Waiting for approval", value:1}, {label:"Completed", value:2}];
  totalNumber: number;
  transactions: Transaction[];
  stockAdjustmentTableData: StockAdjustmentTableData[]=[];
  search: string = "";
  pageIndex = 1;
  pageSize = 10;
  fromDate= Utility.getFirstDateOfCurrentMonth();
  toDate= new Date().toISOString();


  public transactionPtableSettings: IPTableSetting<colDef<keyof StockAdjustmentTableData>> = {
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
    pageSize: this.pageSize,
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
      from:this.fromDate,
      to:this.toDate,
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
    return Utility.getPaginationStatus(this.totalNumber, this.pageIndex, this.pageSize,this.stockAdjustmentTableData.length);
  }


  getTransactions() {
    

    this.inventoryManagementService.getSalesPointStockAdjustmentTransactions(this.pageIndex,this.pageSize,this.search,this.fromDate,this.toDate,this.transactionPtableSettings.selectedDropdownValue).subscribe((res) => {
      this.stockAdjustmentTableData = [];
      this.transactions = res.data;
      this.totalNumber = res.count;
      this.paging.totalRecords = this.totalNumber;
      this.mapTransationsToTableData();
    })
  }
  mapTransationsToTableData() {

    this.stockAdjustmentTableData = this.transactions.map(x => {
      const data = new StockAdjustmentTableData();
      data.id = x.id;
      data.transactionNumber = x.transactionNumber;
      data.transactionDate = Utility.getDateToStringFormat(x.transactionDateStr);
      data.totalIncrease = x.salesPointAdjustmentItems.filter(item=>item.systemQuantity<item.adjustedQuantity).reduce<number>((total,i)=>total+i.adjustedQuantity-i.systemQuantity,0);
      data.totalDecrease = x.salesPointAdjustmentItems.filter(item=>item.adjustedQuantity<item.systemQuantity).reduce<number>((total,i)=>total+i.systemQuantity-i.adjustedQuantity,0);
      data.lines = x.salesPointAdjustmentItems.length;
      data.remarks = x.remarks;
      data.isConfirmed = x.isConfirmed;
      data.disableCheck = !!x.isConfirmed;
      data.disableView = !x.isConfirmed;
      data.disableEdit = x.isConfirmed;
      data.status = this.getTransactionStatus(x);
    
      return data;
    });
  }

  getTransactionStatus(t: Transaction) {
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

  getWorkFlowStatus(t: Transaction): string {
    let result = 'Waiting for approval'
    if (t.transactionWorkflow  == null)
        return result;
    else if (t.transactionWorkflow && t.transactionWorkflow.user) {
      let {user} = t.transactionWorkflow;
      if (t.transactionWorkflow.twStatus === TWStatus.Pending ) {
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
      else if (t.transactionWorkflow.twStatus ===  TWStatus.Rejected){
        result = `Rejected  by ${role.name}`;
        return result;
      }
    }

  }

  paginate(event) {
    this.pageIndex = Number(event.page) + 1;
    this.getTransactions();
    this.transactionPtableSettings.serverSitePageIndex = this.pageIndex;
  }


  addNewStockAdjust() {
    this.router.navigate([RoutesLaout.Inventory,RoutesInventoryManagement.SalesPointStockAdjustment]);
  }

  confirmTransaction(id: number) {
    this.alert.confirm("Are you sure you want to Confirm this transaction ?", () => {
      this.inventoryManagementService.confirmSalesPointStockAdjustmentTransaction(id).subscribe(
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
      this.router.navigate([RoutesLaout.Inventory,RoutesInventoryManagement.SalesPointStockAdjustment, id]);
    }
    if (action === "check-item") {
      this.confirmTransaction(id);
    }
    if (action === "edit-item") {
      this.router.navigate([RoutesLaout.Inventory,RoutesInventoryManagement.SalesPointStockAdjustment, id]);
    }
  }

  fnTransactionPtableCellClick(event) {
    if (event.cellName !== "details") return;
  }

  fnSearch($event: any) {
    this.search = $event.searchVal;
    this.paging.changePage(0);
  }

  handlePazeSizeChange(pageSize: number) {
    console.log(pageSize);
    this.pageSize = pageSize;
    this.paging.changePage(0);
  }
  
  dropDownChange(selected:string){
    this.paging.changePage(0);
  }

  handleDateRange(dateRange:IDateRange){
    Utility.adjustDateRange(dateRange);
    this.fromDate = dateRange.from;
    this.toDate = dateRange.to;
    this.paging.changePage(0);  
  }
}
