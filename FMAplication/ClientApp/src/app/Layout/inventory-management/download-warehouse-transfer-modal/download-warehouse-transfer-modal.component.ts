import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NodeTree } from 'src/app/Shared/Entity';
import { WareHouse } from 'src/app/Shared/Entity/Inventory';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { UserService } from 'src/app/Shared/Services/Users';
import { InventoryManagementService } from '../inventory-management.service';
import * as moment from 'moment';
import { DownloadExcelForWarehouseTransfer } from 'src/app/Shared/Entity/wareHouse';
import { WareHouseService } from 'src/app/Shared/Services/DailyActivity/warehouse.service';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';

@Component({
  selector: 'app-download-warehouse-transfer-modal',
  templateUrl: './download-warehouse-transfer-modal.component.html',
  styleUrls: ['./download-warehouse-transfer-modal.component.css']
})
export class DownloadWarehouseTransferModalComponent implements OnInit {

  wareHouses: WareHouse[] = []
  selectedFromWareHouse: WareHouse;
  selecteToWareHouse: WareHouse;
  isLoadingWareHouses = true;
  nodeTree:NodeTree[]=[];

  constructor(public activeModal: NgbActiveModal,
    private inventoryManagementService: InventoryManagementService,
    private userService:UserService,
    private commonService: CommonService,
    private wareHouseService:WareHouseService,
    private alertService:AlertService) { }

  ngOnInit() {
    this.inventoryManagementService.getWareHouses().subscribe(data => {
        this.wareHouses = data;
        this.isLoadingWareHouses = false;
        this.userService.getNodeTreeByCurrentUser().subscribe(data=>{
          this.nodeTree = data;
          this.selectedFromWareHouse = this.wareHouses[0];
          this.selecteToWareHouse = this.wareHouses[1];
        })
    });
  }


  public downloadFormat() {
    const payload = new DownloadExcelForWarehouseTransfer();
    payload.fromWareHouseId = this.selectedFromWareHouse.id;
    payload.toWareHouseId = this.selecteToWareHouse.id;
    if(payload.toWareHouseId === payload.fromWareHouseId){
      //this.activeModal.close(this.selectedFromWareHouse);
      this.alertService.tosterDanger("Source and destination cannot be same");
      return;
    }
    let fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    let dateTime = moment().format("D MMMM YYYY-HHmm");
    const fileName = `CM360-Warehouse Stock Transfer-${this.selectedFromWareHouse.code}-${dateTime}.xlsx`
    this.wareHouseService.downloadWareHouseTransferExcel(payload).toPromise().then(data => {
        this.commonService.DownloadFile(data, fileName, fileType);
        this.activeModal.close(this.selectedFromWareHouse);
    });
}



}
