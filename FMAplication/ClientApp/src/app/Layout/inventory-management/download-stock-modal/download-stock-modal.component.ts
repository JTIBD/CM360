import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Utility } from 'src/app/Shared/utility';
import { WareHouse } from '../../../Shared/Entity/Inventory';
import { CommonService } from '../../../Shared/Services/Common/common.service';
import { InventoryManagementService } from '../inventory-management.service';

@Component({
  selector: 'app-download-stock-modal',
  templateUrl: './download-stock-modal.component.html',
  styleUrls: ['./download-stock-modal.component.css']
})
export class DownloadStockModalComponent implements OnInit {
    isLoadingWareHouses = true;
    wareHouses: WareHouse[] = []
    selectedWareHouse: WareHouse;
    constructor(public activeModal: NgbActiveModal,
        private inventoryManagementService: InventoryManagementService,
        private commonService: CommonService) { }

    ngOnInit() {
        this.inventoryManagementService.getWareHouses().subscribe(data => {
            this.wareHouses = data;
            this.isLoadingWareHouses = false;
            this.selectedWareHouse = this.wareHouses[0];
        });
    }

    public downloadFormat() {
        let fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        this.inventoryManagementService.downloadStockCreationExcel(this.selectedWareHouse.id).toPromise().then(data => {
            const fileName = `CWStockAdd_${Utility.getDateTimeSuffixForExcelFilename()}.xlsx`;
            this.commonService.DownloadFile(data, fileName, fileType);
            this.activeModal.close(this.selectedWareHouse);
        });
    }

}
