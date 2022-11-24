import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Paginator } from 'primeng/paginator';
import { StockDistributionTableData, PosmProduct } from 'src/app/Shared/Entity';
import { WareHouse } from 'src/app/Shared/Entity/Inventory';
import { WareHouseTranferDetailsTableData, WareHouseTransfer, WareHouseTransferItem } from 'src/app/Shared/Entity/wareHouse';
import { WareHouseReceivedTransfer } from 'src/app/Shared/Entity/wareHouse/wareHouseReceivedTransfer';
import { WareHouseTranserTableData } from 'src/app/Shared/Entity/wareHouse/wareHouseTranserTableData';
import { TransactionStatus, TransactionStatusStrs } from 'src/app/Shared/Enums/TransactionStatus';
import { IDateRange, IParsedExcel, IParseExcelModel } from 'src/app/Shared/interfaces';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { colDef, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { WareHouseService } from 'src/app/Shared/Services/DailyActivity/warehouse.service';
import { FileUtilityService } from 'src/app/Shared/Services/fileUtility/file-utility.service';
import { PosmProductService } from 'src/app/Shared/Services/Product/posmproduct.service';
import { Utility } from 'src/app/Shared/utility';
import { DownloadWarehouseTransferModalComponent } from '../download-warehouse-transfer-modal/download-warehouse-transfer-modal.component';
import { InventoryManagementService } from '../inventory-management.service';

type viewType =  "default"|"transactionDetails";

@Component({
  selector: 'app-ware-house-transfer',
  templateUrl: './ware-house-transfer.component.html',
  styleUrls: ['./ware-house-transfer.component.css']
})
export class WareHouseTransferComponent implements OnInit {

  posmProducts:PosmProduct[]=[];
  excelData: IParsedExcel;
  isEditing = false;
  remark: string;
  selectedDate: string = Utility.getDateToStringFormat(new Date().toISOString());
  transfers: WareHouseTransfer[];
  receivedTransfers: WareHouseReceivedTransfer[];
  stockTransferTableData: WareHouseTranserTableData[] = [];
  stockDistributionRowDetailsTableData: WareHouseTranferDetailsTableData[] = [];
  selectedTransaction: WareHouseTransfer = null;
  selectedView: viewType = "default"
  ptableSummery: object;

  search: string = "";
  pageIndex = 1;
  pageSize = 10;
  total: number;

  date = new Date();

  fromDate = new Date(this.date.getFullYear(), this.date.getMonth(), 1).toISOString();
  toDate = new Date().toISOString();

  @ViewChild("paging", { static: false }) paging: Paginator;


  public transactionPtableSettings: IPTableSetting<colDef<keyof WareHouseTranserTableData>> = {
    tableID: "transactionPtable",
    tableClass: "table table-border ",
    tableName: "Warehouse Stock Transfer",
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

  public transactionDetailsPtableSettings: IPTableSetting<colDef<keyof WareHouseTranferDetailsTableData>> = {
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
      /*this.closeResult = `Closed with: ${result}`;
      this.router.navigate(['/users/users-list/']);*/
    },
      (reason) => {
      });
  }

  handleUpload(e: Event) {
    const target = e.target as HTMLInputElement;
    const file = target.files[0];
    (e.target as any).value="";
    if (file) {
      let model: IParseExcelModel = {
        file
      }
      this.fileUtilityService.ParseExcel(model).subscribe((data) => {
        this.excelData = data;
        this.createTransactions(data);
      });
    }
  }

  getTransactionDate(dateStr: string) {
    return Utility.getDateToStringFormat(dateStr);
  }

  mapSelectedTransactionToDetailsTableData() {
    let data: WareHouseTranferDetailsTableData[] = [];
    data = this.selectedTransaction.items.map(x => {
      const item = new WareHouseTranferDetailsTableData();
      item.posmProductName = x.posmProduct.name;
      item.quantity = x.quantity;
      return item;
    });
    this.stockDistributionRowDetailsTableData = data;

  }

  mapTranfers(){
    this.stockTransferTableData = this.transfers.map((x) => {
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
      data.totalQuantity = x.items.reduce((total, item) => total + item.quantity, 0);
      data.line = x.items.length;
      data.isConfirmed = x.isConfirmed;
      data.disableCheck = !!x.isConfirmed;
      if (x.transactionStatus === TransactionStatus.Pending && !!x.isConfirmed) data.status = "Confirmed";
      else data.status = TransactionStatusStrs[x.transactionStatus];

      data.transferType="Transfer";
      return data;
    })
  }

  mapTransationsToTableData() {
    this.mapTranfers();
  }

  async createTransactions(excelData: IParsedExcel) {

    const posmNames: string[] = excelData.rows[0].slice(4);
    const transfer=new WareHouseTransfer();
    
    let r = excelData.rows[1];
    const wareHouses:WareHouse[]= await this.getWareHouses([r[0],r[2]]);
    let items: WareHouseTransferItem[] = [];
    transfer.fromWarehouseId = wareHouses.find(x=> x.code === r[0]).id;
    transfer.toWarehouseId = wareHouses.find(x=> x.code === r[2]).id;      
    let quantitiesStr = r.slice(4);
    let quantities:number[] = [];

    for(let i=0;i<quantitiesStr.length;i++){
      const q = quantitiesStr[i];
      if(isNaN(q)) {
        this.alertService.tosterDanger(`${q} is not a valid number`);
        return;
      }
      if(q) {
        quantities.push(Number(q));
        let item = new WareHouseTransferItem();
        item.posmProductId = this.posmProducts.find(x=>x.name === posmNames[i]).id;
        item.quantity = q;        
        items.push(item);
      }
    }
    transfer.items = items;

    transfer.items = transfer.items.filter(x=>!!x.quantity);
    this.validateQuantity(transfer.items);
    if (!transfer.items.length) {
      this.alertService.tosterDanger("No products found to distribute");
      return;
    }

    this.wareHouseService.createTransfer(transfer).subscribe(data => {
      this.reset()
      this.alertService.tosterInfo("Successfully created transaction");
    }, (err) => {
      this.alertService.tosterDanger(err.error);
    });
  }

  validateQuantity(items:WareHouseTransferItem[]){
    let negativeValueItems = items.filter(x=>x.quantity < 0);
    if(negativeValueItems.length){
      let posms = this.posmProducts.filter(posm => negativeValueItems.some(i=>i.posmProductId == posm.id));
      let productNames = posms.map(x=>x.name).join(", ");
      this.alertService.tosterDanger(`Negative value found for product ${productNames}`);
      throw "negative quantity";
    }
  }

  fnTransactionPtableCellClick(event) {
    if (event.cellName !== "details") return;
    console.log(event);
    let row: StockDistributionTableData = event.record;
    this.selectedTransaction = this.transfers.find(x => x.id === row.transactionId);
    if (!this.selectedTransaction) return;
    this.mapSelectedTransactionToDetailsTableData();
    this.selectedView = "transactionDetails";
  }

  async getTransfers() {    
      const res = await this.wareHouseService.getTransfers(this.pageIndex, this.pageSize, this.search || "", this.fromDate, this.toDate).toPromise();
      this.transfers = res.data;
      this.total = res.count;
      this.mapTranfers();
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

  haneleCustomActivityOnRecord({ action, record}:{ action:string, record:WareHouseTranserTableData}) {
    console.log(action, record);
    if (action === "view-item") {
      this.selectedTransaction = this.transfers.find(x => record.id === x.id);
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
      let selectedTransfer = this.transfers.find(x => record.id === x.id);
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

}
