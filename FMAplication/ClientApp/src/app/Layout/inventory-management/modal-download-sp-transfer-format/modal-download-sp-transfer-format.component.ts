import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NodeTree, SalesPoint } from 'src/app/Shared/Entity';
import { DownloadExcelForSalesPointTransfer, DownloadExcelForStockDistributions } from 'src/app/Shared/Entity/Inventory';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { UserService } from 'src/app/Shared/Services/Users';
import { InventoryManagementService } from '../inventory-management.service';
import * as moment from 'moment';
import { SalesPointService } from 'src/app/Shared/Services/Sales';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';

@Component({
  selector: 'app-modal-download-sp-transfer-format',
  templateUrl: './modal-download-sp-transfer-format.component.html',
  styleUrls: ['./modal-download-sp-transfer-format.component.css']
})
export class ModalDownloadSpTransferFormatComponent implements OnInit {

  sourceSalesPoints: SalesPoint[] = []
  selectedSourceSalesPoint: SalesPoint;
  isLoadingWareHouses = true;
  nodeTree:NodeTree[]=[];

  salesPoints:SalesPoint[]=[];
  salesPointIds:number[]=[]

  constructor(public activeModal: NgbActiveModal,
    private salesPointService:SalesPointService,
    private alertService: AlertService,
    private userService:UserService,
    private commonService: CommonService) { }

  ngOnInit() {

      this.userService.getNodeTreeByCurrentUser().subscribe(data=>{
        this.nodeTree = data;
        this.selectedSourceSalesPoint = this.sourceSalesPoints[0];
      });
      this.userService.getAllSalesPointByCurrentUser().subscribe(res=>{
        console.log(res.data);
        this.sourceSalesPoints = res.data;        
      })
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
    const payload = new DownloadExcelForSalesPointTransfer();
    payload.fromSalesPointId = this.selectedSourceSalesPoint.id;
    payload.toSalesPointIds = this.getSelectedSalesPointIds();
    if(!payload.toSalesPointIds.length){
      this.alertService.tosterDanger("No destination salespoint selected.");
      return;
    }
    let fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    let dateTime = moment().format("D MMMM YYYY-HHmm");
    const fileName = `CM360-Salespoint Transfer-${this.selectedSourceSalesPoint.code}-${dateTime}.xlsx`
    this.salesPointService.downloadSPTransferExcel(payload).toPromise().then(data => {
        this.commonService.DownloadFile(data, fileName, fileType);
        this.activeModal.close(this.selectedSourceSalesPoint);
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
