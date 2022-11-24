import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { IPosmProductStock } from 'src/app/Shared/Entity/Inventory/StockProduct';
import { SalesPointService } from 'src/app/Shared/Services/Sales';
import { InventoryManagementService } from '../inventory-management.service';

@Component({
  selector: 'app-modal-skupicker',
  templateUrl: './modal-skupicker.component.html',
  styleUrls: ['./modal-skupicker.component.css']
})
export class ModalSKUPickerComponent implements OnInit {

    productData: IPosmProductStock[] = [];
    @Input() selectedWareHouse:number; 
    @Input() selectedSalesPointId:number; 
    @Input() skuSource:"salespoint"|"warehouse"="warehouse"; 
    selectAll:boolean;
  
  constructor(public activeModal: NgbActiveModal, private inventoryManagementService:InventoryManagementService,
    private salesPointService:SalesPointService,) {
   
   }
  
   selectAllChange(eve: any) {
    this.changeProductIsSelectedStatus(eve);
    this.selectAll = !this.selectAll;
  }

   getStockDataByWareHouse(){
       this.inventoryManagementService.getWarehouseStockData(this.selectedWareHouse).subscribe((data: IPosmProductStock[]) => {
           data.forEach((d: IPosmProductStock)=> {
               let product = d;
               product.quantity = d.quantity;
               if(product.quantity <0) product.quantity =0;
               if(product.availableQuantity < 0) product.availableQuantity = 0;
               product.adjustedQuantity = 0;
               this.productData.push(product);
           });
    });
   }

   getStocksBySalesPoint(){
    this.salesPointService.getStocks(1,99999,"",[this.selectedSalesPointId]).subscribe(res=>{
      this.productData = res.data.map(x=>{
        const stockItem:IPosmProductStock = {
          ...x.posmProduct,
          quantity:x.quantity>0?x.quantity:0,
          adjustedQuantity:0,
          availableQuantity:x.availableQuantity>0? x.availableQuantity:0,
                    
        }
        return stockItem;
      })    
    })
   }

  

    ngOnInit() {
        if(this.skuSource === "warehouse") this.getStockDataByWareHouse();
        if(this.skuSource === "salespoint") this.getStocksBySalesPoint();
    }

  

  

  private changeProductIsSelectedStatus(status: boolean) {
    this.productData.forEach(data => {
      data.isSelected =  status;
    })
    
  }

    selectedItems() {
        const selectedItems = this.productData.filter(x => x.isSelected);
        let result = {
            products: selectedItems, 
            selectedWarehouseId: this.selectedWareHouse
        }
        this.activeModal.close(result);
  }

  

}
