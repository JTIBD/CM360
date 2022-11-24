import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Paginator } from 'primeng/paginator';
import { SalesPointReceivedTransfer, SalesPointReceivedTransferDetailsTableData, SalesPointReceivedTransferTableData } from 'src/app/Shared/Entity';
import { TransactionStatus, TransactionStatusStrs } from 'src/app/Shared/Enums/TransactionStatus';
import { IParsedExcel, IDateRange } from 'src/app/Shared/interfaces';
import { IPTableSetting, colDef } from 'src/app/Shared/Modules/p-table';
import { RoutesInventoryManagement } from 'src/app/Shared/Routes/RoutesInventoryManagement';
import { RoutesLaout } from 'src/app/Shared/Routes/RoutesLaout';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { SalesPointService } from 'src/app/Shared/Services/Sales';
import { Utility } from 'src/app/Shared/utility';

type viewType =  "default"|"transactionDetails";

@Component({
  selector: 'app-salespoint-received-transfers',
  templateUrl: './salespoint-received-transfers.component.html',
  styleUrls: ['./salespoint-received-transfers.component.css']
})
export class SalespointReceivedTransfersComponent implements OnInit {

  excelData: IParsedExcel;
  isEditing = false;
  remark: string;
  selectedDate: string = Utility.getDateToStringFormat(new Date().toISOString());
  receivedTransfers: SalesPointReceivedTransfer[];
  stockTransferTableData: SalesPointReceivedTransferTableData[] = [];
  stockDistributionRowDetailsTableData: SalesPointReceivedTransferDetailsTableData[] = [];
  selectedTransaction = new SalesPointReceivedTransfer();
  selectedView: viewType = "default"
  ptableSummery: object;

  search: string = "";
  pageIndex = 1;
  pageSize = 10;
  total: number;
  transferTotal:number;
  receivedTotal:number;

  date = new Date();

  fromDate = new Date(this.date.getFullYear(), this.date.getMonth(), 1).toISOString();
  toDate = new Date().toISOString();

  @ViewChild("paging", { static: false }) paging: Paginator;


  public transactionPtableSettings: IPTableSetting<colDef<keyof SalesPointReceivedTransferTableData>> = {
    tableID: "transactionPtable",
    tableClass: "table table-border ",
    tableName: "Salespoint Received Transfer",
    tableRowIDInternalName: "id",
    tableColDef: [
      {
        headerName: "Source SalesPoint",
        width: "15%",
        internalName: "fromSalesPoint",
        sort: true,
        type: "",
      },
      {
        headerName: "Destination SalesPoint",
        width: "15%",
        internalName: "toSalesPoint",
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
        internalName: "line",
        sort: true,
        type: "",
      },
      {
        headerName: "Quantity",
        width: "10%",
        internalName: "totalQuantity",
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
        width: "10%",
        internalName: "status",
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
    tableFooterVisibility: false,
    enabledViewBtn: true,
    enabledCheckBtn: true,
    enabledServerSitePaggination: true,
    enablePazeSizeSelection: true,
    enabledDataLength: true,
    enableDateRangeFilter: true,
    intialDateRange: {
      from: this.fromDate,
      to: this.toDate,
    },

  };

  detailsTablePazeSize = 5;

  public transactionDetailsPtableSettings: IPTableSetting<colDef<keyof SalesPointReceivedTransferDetailsTableData>> = {
    tableID: "transactionDetailsPtable",
    tableClass: "table table-border ",
    tableName: "Transaction Details",
    tableRowIDInternalName: "id",
    tableColDef: [
      {
        headerName: "POSM Name",
        width: "20%",
        internalName: "posmProductName",
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
    //enabledAutoScrolled:true,
    //enabledCellClick: true,
    enabledColumnFilter: true,
    enabledDeleteBtn: false,
    enabledPrint: true,

  };

  permissionGroup: PermissionGroup = new PermissionGroup();
  private initPermissionGroup() {
      this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
  }

  constructor(private modalService: NgbModal,
    private salesPointService:SalesPointService,
    private router:Router,
    private activatedRoute: ActivatedRoute,
    private activityPermissionService: ActivityPermissionService) {
      this.initPermissionGroup();
    }

  ngOnInit() {
    this.getData();
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
    return Utility.getPaginationStatus(this.total, this.pageIndex, this.pageSize,this.stockTransferTableData.length);
  }


  async getData(){
    this.getTransfers();
  }

  getTransactionDate(dateStr: string) {
    return Utility.getDateToStringFormat(dateStr);
  }

  mapSelectedTransactionToDetailsTableData() {
    let data: SalesPointReceivedTransferDetailsTableData[] = [];
    data = this.selectedTransaction.items.map(x => {
      const item = new SalesPointReceivedTransferDetailsTableData();
      item.posmProductName = x.posmProduct.name;
      item.quantity = x.quantity;
      item.receivedQuantity = x.receivedQuantity;
      return item;
    });
    this.stockDistributionRowDetailsTableData = data;

  }

  mapReceivedTransfer(){
    this.stockTransferTableData = this.receivedTransfers.map((x) => {
      const data = new SalesPointReceivedTransferTableData();
      data.id = x.id;
      if(x.fromSalesPoint){        
        data.fromSalesPoint =`${x.fromSalesPoint.name}(${x.fromSalesPoint.code})` ;
      }
      if(x.toSalesPoint){        
        data.toSalesPoint = `${x.toSalesPoint.name}(${x.toSalesPoint.code})`;
      }
      data.transactionNumber = x.transactionNumber;
      data.date = Utility.getDateToStringFormat(x.transactionDateStr);
      data.totalQuantity = x.items.reduce((total, item) => total + item.receivedQuantity, 0);
      data.line = x.items.length;
      data.isConfirmed = x.isConfirmed;
      data.disableCheck = !!x.isConfirmed;
      if (x.transactionStatus === TransactionStatus.Pending && !!x.isConfirmed) data.status = "Confirmed";
      else data.status = TransactionStatusStrs[x.transactionStatus];
      return data;
    });    
  }

  mapTransationsToTableData() {
    this.mapReceivedTransfer();
  }

  fnTransactionPtableCellClick(event) {
    if (event.cellName !== "details") return;
    let row: SalesPointReceivedTransferTableData = event.record;
    this.selectedTransaction = this.receivedTransfers.find(x => x.id === row.id);
    if (!this.selectedTransaction) return;
    this.mapSelectedTransactionToDetailsTableData();
    this.selectedView = "transactionDetails";
  }

  async getTransfers() {    
    try{     
      const res2 = await this.salesPointService.getReceivedTransfers(this.pageIndex,this.pageSize,this.search||"",this.fromDate,this.toDate).toPromise();
      this.receivedTransfers = res2.data;      
      this.total = res2.count;      
    }catch(e){}
    this.mapTransationsToTableData();
    this.paging.updatePaginatorState();
        
  }


  paginate(event) {
    this.pageIndex = Number(event.page) + 1;
    this.getTransfers();
    this.transactionPtableSettings.serverSitePageIndex = this.pageIndex;
  }

  haneleCustomActivityOnRecord({ action, record}:{ action:string, record:SalesPointReceivedTransferTableData}) {
    if (action === "view-item") {
      this.selectedTransaction = this.receivedTransfers.find(x => record.id === x.id);
      this.mapSelectedTransactionToDetailsTableData();
      this.selectedView = "transactionDetails";
      this.ptableSummery = {
        "Source Salespoint": this.selectedTransaction.fromSalesPoint.name,
        "Destination Salespoint": this.selectedTransaction.toSalesPoint.name,
        "Transaction Id": this.selectedTransaction.transactionNumber,
        "Transaction Date": this.selectedTransaction.transactionDateStr,
      }
    }
  }

  setViewView(viewType: viewType) {
    this.selectedView = viewType;
    if (viewType === "default") {
      this.getTransfers();

    }
  }

  reset() {
    this.toDate = new Date().toISOString();
    this.paging.changePage(0);
  }

  fnSearch($event: any) {
    this.search = $event.searchVal;
    this.paging.changePage(0);
  }

  handlePazeSizeChange(pageSize: number) {
    this.pageSize = pageSize;
    this.paging.changePage(0);
  }

  handleDateRange(dateRange: IDateRange) {
    Utility.adjustDateRange(dateRange);
    this.fromDate = dateRange.from;
    this.toDate = dateRange.to;
    this.paging.changePage(0);

  }

  navigateToReceive(){
    this.router.navigate([RoutesLaout.Inventory,RoutesInventoryManagement.ReceiveSalesPointTransfer]);
  }
}
