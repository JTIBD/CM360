import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SalesPointReceivedTransfer, SalesPointReceivedTransferItem, SalesPointTransfer, SelectedStockToReceiveTablaData } from 'src/app/Shared/Entity';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { IPTableSetting, colDef } from 'src/app/Shared/Modules/p-table';
import { RoutesInventoryManagement } from 'src/app/Shared/Routes/RoutesInventoryManagement';
import { RoutesLaout } from 'src/app/Shared/Routes/RoutesLaout';
import { SalesPointService } from 'src/app/Shared/Services/Sales';
import { Utility } from 'src/app/Shared/utility';

@Component({
  selector: 'app-receive-salespoint-transfer',
  templateUrl: './receive-salespoint-transfer.component.html',
  styleUrls: ['./receive-salespoint-transfer.component.css']
})
export class ReceiveSalespointTransferComponent implements OnInit {

  selectedTransactionNumber:string;
  selectedTransaction:SalesPointTransfer;
  transactionIds:string[]=[];
  distributions:SalesPointTransfer[]=[];
  reference:string="";
  remarks:string="";
  selectedTransactionDate:string="";
  receivedDate:string = Utility.getDateToStringFormat(new Date().toISOString());
  destinationSalesPoint:string="";

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

  constructor(private alertService: AlertService,
    private router: Router,
    private salesPointService:SalesPointService,
    //private modalService: NgbModal,
    ) { }

  ngOnInit() {
    this.salesPointService.getReceivableSPTransfersByCurrentUser().subscribe(res=>{
      this.distributions = res;
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
    if(!!this.selectedTransaction.toSalesPoint) this.destinationSalesPoint = this.selectedTransaction.toSalesPoint.name
    
    this.mapTableData();
  
  }

  handleReceive(){
    const payload =  new SalesPointReceivedTransfer();
    payload.remarks = this.remarks;
    payload.sourceTransferId = this.selectedTransaction.id;
    payload.toSalesPointId = this.selectedTransaction.toSalesPointId;
    payload.fromSalesPointId = this.selectedTransaction.fromSalesPointId;    
    const items:SalesPointReceivedTransferItem[] =[];
    
    for (let wDis of this.selectedStockTableData){
      const data = new SalesPointReceivedTransferItem();
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
    
    this.salesPointService.recieveTransfer(payload).subscribe(()=>{
      this.alertService.tosterInfo("Receive stock successfull");
      this.router.navigate([RoutesLaout.Inventory,RoutesInventoryManagement.SalesPointReceivedTransfer]);
    })
  }

  backToMain() { 
    this.router.navigate([RoutesLaout.Inventory,RoutesInventoryManagement.SalesPointReceivedTransfer]);
  }
}
