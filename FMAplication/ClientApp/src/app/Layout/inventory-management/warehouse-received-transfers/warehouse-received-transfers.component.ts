import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Paginator } from 'primeng/paginator';
import { PosmProduct } from 'src/app/Shared/Entity';
import { WareHouseReceivedTransfer, WareHouseReceivedTranserTableData, WareHouseReceivedTranferDetailsTableData } from 'src/app/Shared/Entity/wareHouse';
import { WareHouseTranserTableData } from 'src/app/Shared/Entity/wareHouse/wareHouseTranserTableData';
import { TransactionStatus, TransactionStatusStrs } from 'src/app/Shared/Enums/TransactionStatus';
import { IParsedExcel, IDateRange } from 'src/app/Shared/interfaces';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { IPTableSetting, colDef } from 'src/app/Shared/Modules/p-table';
import { RoutesInventoryManagement } from 'src/app/Shared/Routes/RoutesInventoryManagement';
import { RoutesLaout } from 'src/app/Shared/Routes/RoutesLaout';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { WareHouseService } from 'src/app/Shared/Services/DailyActivity/warehouse.service';
import { FileUtilityService } from 'src/app/Shared/Services/fileUtility/file-utility.service';
import { PosmProductService } from 'src/app/Shared/Services/Product/posmproduct.service';
import { Utility } from 'src/app/Shared/utility';
import { DownloadWarehouseTransferModalComponent } from '../download-warehouse-transfer-modal/download-warehouse-transfer-modal.component';

type viewType =  "default"|"transactionDetails";


@Component({
  selector: 'app-warehouse-received-transfers',
  templateUrl: './warehouse-received-transfers.component.html',
  styleUrls: ['./warehouse-received-transfers.component.css']
})
export class WarehouseReceivedTransfersComponent implements OnInit {

  posmProducts:PosmProduct[]=[];
  excelData: IParsedExcel;
  isEditing = false;
  remark: string;
  selectedDate: string = Utility.getDateToStringFormat(new Date().toISOString());
  receivedTransfers: WareHouseReceivedTransfer[];
  stockTransferTableData: WareHouseReceivedTranserTableData[] = [];
  stockDistributionRowDetailsTableData: WareHouseReceivedTranferDetailsTableData[] = [];
  selectedTransaction = new WareHouseReceivedTransfer();
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


  public transactionPtableSettings: IPTableSetting<colDef<keyof WareHouseReceivedTranserTableData>> = {
    tableID: "transactionPtable",
    tableClass: "table table-border ",
    tableName: "Warehouse Received Transfer",
    tableRowIDInternalName: "id",
    tableColDef: [
      {
        headerName: "Source Warehouse",
        width: "15%",
        internalName: "fromWareHouseName",
        sort: true,
        type: "",
      },
      {
        headerName: "Destination Warehouse",
        width: "15%",
        internalName: "toWareHouseName",
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

  public transactionDetailsPtableSettings: IPTableSetting<colDef<keyof WareHouseReceivedTranferDetailsTableData>> = {
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
    private fileUtilityService: FileUtilityService,
    private alertService: AlertService,
    private wareHouseService:WareHouseService,
    private posmProductService: PosmProductService,
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
    await this.getPosmProducts();
  }

  openExcelImportModal() {
    let ngbModalOptions: NgbModalOptions = {
      backdrop: 'static',
      keyboard: false,
      size: 'lg'
    };
    const modalRef = this.modalService.open(DownloadWarehouseTransferModalComponent, ngbModalOptions);

    modalRef.result.then((result) => {

    },
      (reason) => {
      });
  }

  getTransactionDate(dateStr: string) {
    return Utility.getDateToStringFormat(dateStr);
  }

  mapSelectedTransactionToDetailsTableData() {
    let data: WareHouseReceivedTranferDetailsTableData[] = [];
    data = this.selectedTransaction.items.map(x => {
      const item = new WareHouseReceivedTranferDetailsTableData();
      item.posmProductName = x.posmProduct.name;
      item.quantity = x.quantity;
      item.receivedQuantity = x.receivedQuantity;
      return item;
    });
    this.stockDistributionRowDetailsTableData = data;

  }

  mapReceivedTransfer(){
    this.stockTransferTableData = this.receivedTransfers.map((x) => {
      const data = new WareHouseTranserTableData();
      data.id = x.id;
      if(x.fromWarehouse){
        data.fromWareHouseCode = x.fromWarehouse.code;
        data.fromWareHouseName =`${x.fromWarehouse.name}(${x.fromWarehouse.code})` ;
      }
      if(x.toWarehouse){
        data.toWareHouseCode = x.toWarehouse.name;
        data.toWareHouseName = `${x.toWarehouse.name}(${x.toWarehouse.code})`;
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
    console.log(event);
    let row: WareHouseReceivedTranserTableData = event.record;
    this.selectedTransaction = this.receivedTransfers.find(x => x.id === row.id);
    if (!this.selectedTransaction) return;
    this.mapSelectedTransactionToDetailsTableData();
    this.selectedView = "transactionDetails";
  }

  async getTransfers() {    
    try{     
      const res = await this.wareHouseService.getReceivedTransfers(this.pageIndex,this.pageSize,this.search||"",this.fromDate,this.toDate).toPromise();
      this.receivedTransfers = res.data;      
      this.total = res.count;      
    }catch(e){}
    this.mapTransationsToTableData();
    this.paging.updatePaginatorState();
        
  }

  async getPosmProducts(){
    try{
      const response = await this.posmProductService.getPosmProductList().toPromise();
      this.posmProducts= response.data.model;
    }catch(e){
      console.log(e);
    }
  }

  async getWareHouses(codes:string[]){
    try{
      const response = await this.wareHouseService.getWareHousesByCodes(codes).toPromise();
      return response;
    }catch(e){
      console.log(e);
    }
  }

  paginate(event) {
    this.pageIndex = Number(event.page) + 1;
    this.getTransfers();
    this.transactionPtableSettings.serverSitePageIndex = this.pageIndex;
  }

  haneleCustomActivityOnRecord({ action, record}:{ action:string, record:WareHouseReceivedTranserTableData}) {
    console.log(action, record);
    if (action === "view-item") {
      this.selectedTransaction = this.receivedTransfers.find(x => record.id === x.id);
      this.mapSelectedTransactionToDetailsTableData();
      this.selectedView = "transactionDetails";
      this.ptableSummery = {
        "From ware house": this.selectedTransaction.fromWarehouse.name,
        "To ware house": this.selectedTransaction.toWarehouse.name,
        "Transaction Id": this.selectedTransaction.transactionNumber,
        "Transaction Date": this.selectedTransaction.transactionDateStr,
      }
    }
    if (action === "check-item") {
      let selectedTransfer = this.receivedTransfers.find(x => record.id === x.id);
      this.alertService.confirm("Are you sure you want to confirm this item?", () => {
        this.wareHouseService.confirmTransfer(selectedTransfer.id).subscribe(data => {
          selectedTransfer.isConfirmed = true;
          this.mapTransationsToTableData();
          this.alertService.tosterSuccess("Successfully confirmed.");
        }, (err) => {
          console.log(err.error);
        });

      }, () => {

      });

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
    this.router.navigate([RoutesLaout.Inventory,RoutesInventoryManagement.ReceiveWareHouseTransfer]);
  }
}
