import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NodeTree, SalesPoint } from 'src/app/Shared/Entity';
import { DownloadExcelForStockDistributions, WareHouse } from 'src/app/Shared/Entity/Inventory';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { UserService } from 'src/app/Shared/Services/Users';
import { InventoryManagementService } from '../inventory-management.service';
import * as moment from 'moment';

@Component({
  selector: 'app-download-stock-distribution-modal',
  templateUrl: './download-stock-distribution-modal.component.html',
  styleUrls: ['./download-stock-distribution-modal.component.css']
})
export class DownloadStockDistributionModalComponent implements OnInit {

  wareHouses: WareHouse[] = []
  selectedWareHouse: WareHouse;
  isLoadingWareHouses = true;
  nodeTree:NodeTree[]=[];

  salesPoints:SalesPoint[]=[];
  salesPointIds:number[]=[]

  constructor(public activeModal: NgbActiveModal,
    private inventoryManagementService: InventoryManagementService,
    private userService:UserService,
    private commonService: CommonService) { }

  ngOnInit() {
    this.inventoryManagementService.getWareHouses().subscribe(data => {
        this.wareHouses = data;
        this.isLoadingWareHouses = false;
        this.userService.getNodeTreeByCurrentUser().subscribe(data=>{
          this.nodeTree = data;
          this.selectedWareHouse = this.wareHouses[0];
        })
    });
  }

  handleSalesPointSelect(event,salesPoint:SalesPoint){
    salesPoint.isSelected = event.target.checked;
    // if(event.target.checked) this.selectedSalesPointIds.push(salesPointId);
    // else this.selectedSalesPointIds = this.selectedSalesPointIds.filter(x=> x !== salesPointId);
  }

  getSelectedSalesPointIds(){
    const salesPointIds:number[]=[];
    let fun=(tree:NodeTree[])=>{
      tree.forEach(tr=>{
        if(!!tr.salesPoints && !!tr.salesPoints.length){
          tr.salesPoints.forEach(x=>{
            if(x.isSelected) salesPointIds.push(x.salesPointId);
          })
        }
        else if(!!tr.nodes) fun(tr.nodes);
      })
    }
    fun(this.nodeTree);
    return  salesPointIds;
  }

  public downloadFormat() {
    const payload = new DownloadExcelForStockDistributions();
    payload.wareHouseId = this.selectedWareHouse.id;
    payload.salesPointIds = this.getSelectedSalesPointIds();
    let fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    let dateTime = moment().format("D MMMM YYYY-HHmm");
    const fileName = `CM360-Stock Distribution-${this.selectedWareHouse.code}-${dateTime}.xlsx`
    this.inventoryManagementService.downloadStockDistributionExcel(payload).toPromise().then(data => {
        this.commonService.DownloadFile(data, fileName, fileType);
        this.activeModal.close(this.selectedWareHouse);
    });
}
getNodeById(id:number){
  let find=(tree:NodeTree[])=>{
    let node = tree.find(t=>t.node.id === id);
    if(node) return node;
    //@ts-ignore
    else return find(tree.filter(x=>!!x.nodes).map(x=>x.nodes).flat());
  }
  return find(this.nodeTree);

}
handleNodeSelect(item:NodeTree,checked:boolean){
  console.log(item,checked);
  const node:NodeTree = this.getNodeById(item.node.id);
  console.log(node);
  if(!node) return;
  let fun = (trees:NodeTree[],checked:boolean)=>{
    trees.forEach(tr=>{
      tr.isSelected = checked;
      if(!!tr.nodes && tr.nodes.length) fun(tr.nodes,checked);
      else if(!!tr.salesPoints && !!tr.salesPoints.length){
        tr.salesPoints.forEach(sl=>{
          sl.isSelected = checked;
        })
      }
    })
  }
  fun([node],checked);
}

}
