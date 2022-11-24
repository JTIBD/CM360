import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SelectedStockToReceiveTablaData } from 'src/app/Shared/Entity';
import { WareHouseReceivedTransfer, WareHouseReceivedTransferItem, WareHouseTransfer } from 'src/app/Shared/Entity/wareHouse';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { colDef, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { RoutesInventoryManagement } from 'src/app/Shared/Routes/RoutesInventoryManagement';
import { RoutesLaout } from 'src/app/Shared/Routes/RoutesLaout';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { WareHouseService } from 'src/app/Shared/Services/DailyActivity/warehouse.service';
import { FileUtilityService } from 'src/app/Shared/Services/fileUtility/file-utility.service';
import { Utility } from 'src/app/Shared/utility';
import { InventoryManagementService } from '../inventory-management.service';

@Component({
  selector: 'app-receive-warehouse-transfer',
  templateUrl: './receive-warehouse-transfer.component.html',
  styleUrls: ['./receive-warehouse-transfer.component.css']
})
export class ReceiveWarehouseTransferComponent implements OnInit {

  selectedTransactionNumber:string;
  selectedTransaction:WareHouseTransfer;
  transactionIds:string[]=[];
  distributions:WareHouseTransfer[]=[];
  reference:string="";
  remarks:string="";
  selectedTransactionDate:string="";
  receivedDate:string = Utility.getDateToStringFormat(new Date().toISOString());
  destinationWareHouse:string="";

  selectedStockTableData:SelectedStockToReceiveTablaData[]=[];

  public transactionDetailsPtableSettings: IPTableSetting<colDef<string>> = {
    tableID: "transactionDetailsPtable",
    tableClass: "table table-border ",
    tableName: "Transfer Details",
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

  constructor(private inventoryManagementService: InventoryManagementService,
    private commonService: CommonService,
    private fileUtilityService: FileUtilityService,
    private alertService: AlertService,
    private router: Router,
    private wareHouseService:WareHouseService,
    //private modalService: NgbModal,
    ) { }

  ngOnInit() {
    this.wareHouseService.getReceivablePOSMDistributionByCurrentUser().subscribe(x=>{
      this.distributions = x;
    });
  }

  mapTableData(){
    if(!this.selectedTransaction || !this.selectedTransaction.items) return;
    const selectedStockTableData = [];
    this.selectedTransaction.items.forEach(wDis=>{
      const row = new SelectedStockToReceiveTablaData();
      row.posmProductId = wDis.posmProductId;
      row.quantity = wDis.quantity;
      row.receivedQuantity=wDis.quantity;
      if(wDis.posmProduct){
        row.sku = wDis.posmProduct.name;
        row.skuCode = wDis.posmProduct.code;
      }
      selectedStockTableData.push(row);
    })
    this.selectedStockTableData = selectedStockTableData;

  }

  fnChangeTransactionSelecton(id: number) {
    this.selectedTransaction = this.distributions.find(d=>d.id == id);

    this.selectedTransactionDate = Utility.getDateToStringFormat(this.selectedTransaction.transactionDateStr);
    this.selectedTransactionNumber = this.selectedTransaction.transactionNumber;
    if(!!this.selectedTransaction.toWarehouse) this.destinationWareHouse = this.selectedTransaction.toWarehouse.name
    
    this.mapTableData();
  
  }

  handleReceive(){
    const payload =  new WareHouseReceivedTransfer();
    payload.remarks = this.remarks;
    payload.sourceTransferId = this.selectedTransaction.id;
    payload.toWarehouseId = this.selectedTransaction.toWarehouseId;
    payload.fromWarehouseId = this.selectedTransaction.fromWarehouseId;    
    const items:WareHouseReceivedTransferItem[] =[];
    
    for (let wDis of this.selectedStockTableData){
      const data = new WareHouseReceivedTransferItem();
      data.posmProductId= wDis.posmProductId;
      data.quantity = wDis.quantity;      
      if(!isNaN(wDis.receivedQuantity)){
        data.receivedQuantity = Number(wDis.receivedQuantity);
      } 
      else {
        this.alertService.tosterDanger(`${wDis.receivedQuantity} is not a number`);
        return;
      }
      items.push(data);
    }    
    payload.items = items;
    
    this.wareHouseService.recieveTransfer(payload).subscribe(data=>{
      this.alertService.tosterInfo("Receive stock successfull");
      this.router.navigate([RoutesLaout.Inventory,RoutesInventoryManagement.WareHouseReceivedTransfer]);
    })
  }

  backToMain(){
    this.router.navigate([RoutesLaout.Inventory,RoutesInventoryManagement.WareHouseReceivedTransfer]);
  }

}
