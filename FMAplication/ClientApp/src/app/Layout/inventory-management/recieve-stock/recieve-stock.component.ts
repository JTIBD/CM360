import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SelectedStockToReceiveTablaData } from 'src/app/Shared/Entity';
import { Transaction } from 'src/app/Shared/Entity/Inventory/Transaction';
import { WDistributionRecieveTransaction } from 'src/app/Shared/Entity/Inventory/WDistributionRecieveTransaction';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { FileUtilityService } from 'src/app/Shared/Services/fileUtility/file-utility.service';
import { Utility } from 'src/app/Shared/utility';
import { InventoryManagementService } from '../inventory-management.service';

@Component({
  selector: 'app-recieve-stock',
  templateUrl: './recieve-stock.component.html',
  styleUrls: ['./recieve-stock.component.css']
})
export class RecieveStockComponent implements OnInit {

  selectedTransactionNumber:string;
  selectedTransaction:Transaction;
  transactionIds:string[]=[];
  distributions:Transaction[]=[];
  reference:string="";
  remarks:string="";
  selectedTransactionDate:string="";
  receivedDate:string = Utility.getDateToStringFormat(new Date().toISOString());
  selectedTransactionSalesPoint:string="";

  selectedStockTableData:SelectedStockToReceiveTablaData[]=[];

  public transactionDetailsPtableSettings: IPTableSetting = {
    tableID: "transactionDetailsPtable",
    tableClass: "table table-border ",
    tableName: "Transaction Details",
    tableRowIDInternalName: "id",
    tableColDef: [
      { 
        headerName: "POSM Code", 
        width: "15%", 
        internalName: "skuCode",
        sort: true, 
        type: "",
       },
      { 
        headerName: "POSM Name", 
        width: "15%", 
        internalName: "sku", 
        sort: true, 
        type: "",
       },      
       { 
        headerName: "Quantity", 
        width: "10%", 
        internalName: "quantity", 
        sort: true, 
        type: "",
       },
       { 
        headerName: "Received Quantity", 
        width: "10%", 
        internalName: "receivedQuantity", 
        sort: true, 
        type: "text-field",
       },
   
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: 10,
    enabledPagination: true,
    //enabledAutoScrolled:true,
    //enabledCellClick: true,
    enabledColumnFilter: true,
    enabledDeleteBtn: false,
    enabledPrint:false,
    
  };

  permissionGroup: PermissionGroup = new PermissionGroup();
  private initPermissionGroup() {
      this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
  }

  constructor(private inventoryManagementService: InventoryManagementService,
    private commonService: CommonService,
    private fileUtilityService: FileUtilityService,
    private alertService: AlertService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private activityPermissionService: ActivityPermissionService
    //private modalService: NgbModal,
    ) {
      this.initPermissionGroup();
    }

  ngOnInit() {
    this.inventoryManagementService.getReceivablePOSMDistributionByCurrentUser().subscribe(x=>{
      this.distributions = x;
    });
  }

  mapTableData(){
    if(!this.selectedTransaction || !this.selectedTransaction.wDistributionTransactions) return;
    const selectedStockTableData = [];
    this.selectedTransaction.wDistributionTransactions.forEach(wDis=>{
      const row = new SelectedStockToReceiveTablaData();
      row.posmProductId = wDis.posmProductId;
      row.quantity = wDis.quantity;
      row.receivedQuantity=wDis.quantity;
      if(wDis.posmProductModel){
        row.sku = wDis.posmProductModel.name;
        row.skuCode = wDis.posmProductModel.code;
      }
      selectedStockTableData.push(row);
    })
    this.selectedStockTableData = selectedStockTableData;

  }

  fnChangeTransactionSelecton(id: number) {
    this.selectedTransaction = this.distributions.find(d=>d.id == id);

    this.selectedTransactionDate = Utility.getDateToStringFormat(this.selectedTransaction.transactionDateStr);
    this.selectedTransactionNumber = this.selectedTransaction.transactionNumber;
    if(!!this.selectedTransaction.salesPoint) this.selectedTransactionSalesPoint = this.selectedTransaction.salesPoint.name
    
    this.mapTableData();
  
  }

  handleReceive(){
    const payload =  new Transaction();
    payload.remarks = this.remarks;
    payload.referenceTransactionId = this.selectedTransaction.id;
    payload.salesPointId = this.selectedTransaction.salesPointId;
    payload.warehouseId = this.selectedTransaction.warehouseId;    
    const receivedPosms:WDistributionRecieveTransaction[] =[];
    
    for (let wDis of this.selectedStockTableData){
      const data = new WDistributionRecieveTransaction();
      data.posmProductId= wDis.posmProductId;
      data.quantity = wDis.quantity;      
      if(!isNaN(wDis.receivedQuantity)){
        data.recievedQuantity = Number(wDis.receivedQuantity);
      } 
      else {
        this.alertService.tosterDanger(`${wDis.receivedQuantity} is not a number`);
        return;
      }
      receivedPosms.push(data);
    }    
    payload.wDistributionRecieveTransactions = receivedPosms;
    
    this.inventoryManagementService.recievePOSM_Stock(payload).subscribe(data=>{
      this.alertService.tosterInfo("Receive stock successfull");
      this.router.navigate(["/inventory/stock-receives"]);
    })
  }
  backToMain() {
    this.router.navigate(["/inventory/stock-receives"]);
  }

}
