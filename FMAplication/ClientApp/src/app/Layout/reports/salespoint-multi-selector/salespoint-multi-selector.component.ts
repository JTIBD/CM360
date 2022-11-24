import { InventoryManagementService } from './../../inventory-management/inventory-management.service';
import { PosmProductService } from 'src/app/Shared/Services/Product/posmproduct.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NodeHieararchy, NodeTree, PosmProduct, SalesPoint } from 'src/app/Shared/Entity';
import { UserService } from 'src/app/Shared/Services/Users/user.service';
import { Utility } from 'src/app/Shared/utility';
import { ISalesPointMultiSelectSettings, SalesPointMultiSelectItem, SalesPointMultiSelectItemValues } from 'src/app/Shared/Entity/Reports/ISalespointMultiSelectSettings';
import { IDateRange } from 'src/app/Shared/interfaces';
import { WareHouse } from 'src/app/Shared/Entity/Inventory';

@Component({
  selector: 'app-salespoint-multi-selector',
  templateUrl: './salespoint-multi-selector.component.html',
  styleUrls: ['./salespoint-multi-selector.component.css']
})
export class SalespointMultiSelectorComponent implements OnInit {

  @Input() reportName: string;
  @Input() settings: ISalesPointMultiSelectSettings;
  node: NodeHieararchy = new NodeHieararchy();

  selectedItems: SalesPointMultiSelectItem = new SalesPointMultiSelectItem();
  selectedValues = new SalesPointMultiSelectItemValues();
  @Output() exportReportCallbackFn: EventEmitter<SalesPointMultiSelectItemValues> = new EventEmitter<SalesPointMultiSelectItemValues>() || null;
  @Output() dateRangeCallbackFn: EventEmitter<IDateRange> = new EventEmitter<IDateRange>() || null;
  @Output() nodeListCallbackFn: EventEmitter<NodeHieararchy> = new EventEmitter<NodeHieararchy>() || null;


  nodeTree: NodeTree[] = [];
  constructor(private userService: UserService, private commonService: CommonService,
    private posmProductService: PosmProductService, private inventorySevice: InventoryManagementService) { }

  async ngOnInit() {
    await this.userService.getNodeTreeByCurrentUser().toPromise().then((data) => {
          this.nodeTree = data;
          this.node = Utility.getNodeHierarchyFromNodeTree(this.nodeTree);
          this.nodeListCallbackFn.emit(this.node);
          console.log(this.node);
          this.selectAllForGroupBy(this.node.regions);
          this.selectAllForGroupBy(this.node.areas);
          this.selectAllForGroupBy(this.node.teritories);
          this.selectAllForGroupBy(this.node.salespoints);
          this.setTopMostHierarchyDropdownOptions();
    });
   
    if(this.settings.enableDateRangeFilter){
      var year = new Date().getUTCFullYear();
      var month = new Date().getUTCMonth();
      this.selectedItems.selectedFromDate = this.commonService.dateToNgbDate(new Date(year,month,1));
      this.selectedItems.selectedToDate = this.commonService.dateToNgbDate(new Date());
    }

    if(this.settings.enablePosmItems){
      await this.posmProductService.getAllJtiPosmProducts().toPromise().then((response) => {
        this.selectedItems.selectedPosmProducts = response;
        this.selectAllForGroupBy(this.selectedItems.selectedPosmProducts);
      });
    }
    if(this.settings.enableWarehouses){
      await this.inventorySevice.getWareHouses().toPromise().then((data) => {
        this.selectedItems.selectedWarehouses = data;
        this.selectAllForGroupBy(this.selectedItems.selectedWarehouses);
      });
    }
  }

  setTopMostHierarchyDropdownOptions(){
    if(this.node.regions.length){
      this.selectedItems.selectedRegions = this.node.regions;
      return;
    }

    if(this.node.areas.length){
      this.selectedItems.selectedAreas = this.node.areas;
      return;
    }

    if(this.node.teritories.length){
      this.selectedItems.selectedTerritories = this.node.teritories;
      return;
    }

    if(this.node.salespoints.length){
      this.selectedItems.selectedSalesPoints = this.node.salespoints;
      return;
    }
  }

  export(){
    if(this.settings.enableDateRangeFilter) this.formatDateRange();
    this.exportReportCallbackFn.emit(this.selectedValues);
  }

  removeAreas(){
    let filteredAreas = this.node.areas.filter(x => this.selectedValues.selectedRegions.includes(x.node.parentId));
    this.selectedValues.selectedAreas = this.selectedValues.selectedAreas.filter(areaId => 
      filteredAreas.some(y => y.node.nodeId === areaId));
  }

  removeTerritories(){
    let filteredTerritories = this.node.teritories.filter(x => this.selectedValues.selectedAreas.includes(x.node.parentId));
      this.selectedValues.selectedTerritories = this.selectedValues.selectedTerritories.filter(areaId => 
        filteredTerritories.some(y => y.node.nodeId === areaId));
  }


  removeSalespoints(){
    this.selectedItems.selectedSalesPoints = [];
      this.selectedItems.selectedTerritories.map(tr => {
        let salesPoints = tr.salesPoints.filter(sp => 
          this.selectedValues.selectedTerritories.includes(tr.node.nodeId));
          
        if(!!salesPoints && salesPoints.length>0){
          this.selectedItems.selectedSalesPoints.push(...salesPoints);
        }
      });

    this.selectedValues.selectedSalesPoints = this.selectedValues.selectedSalesPoints.filter(x => 
      this.selectedItems.selectedSalesPoints.some(y => y.salesPointId === x));
    }

  onChangeDropdown(event: any, nodeType: any){
    if(nodeType == 'regions'){
      this.selectedItems.selectedAreas = this.node.areas.filter(x => this.selectedValues.selectedRegions.includes(x.node.parentId));
      this.removeAreas();
      this.removeTerritories();
      this.removeSalespoints();
    }
    if(nodeType == 'areas'){
      this.selectedItems.selectedTerritories = this.node.teritories.filter(x => this.selectedValues.selectedAreas.includes(x.node.parentId));
      this.removeTerritories();
      this.removeSalespoints();
    }
    if(nodeType == 'teritories'){
      this.removeSalespoints();
    }
    if(nodeType == 'salespoints'){

    }
  }

  handleDateRangeChange(dateRange: IDateRange){
    if(this.selectedItems.selectedFromDate) dateRange.from = this.commonService.ngbDateToDate(this.selectedItems.selectedFromDate).toISOString();
    if(this.selectedItems.selectedToDate) dateRange.to = this.commonService.ngbDateToDate(this.selectedItems.selectedToDate).toISOString();
    console.log(dateRange);
  }

  formatDateRange(){
    const dateRange={} as IDateRange;
    this.handleDateRangeChange(dateRange);
    Utility.adjustDateRange(dateRange);        
    this.selectedValues.selectedFromDate = dateRange.from;
    this.selectedValues.selectedToDate = dateRange.to;
  }


  selectAllForGroupBy(items: NodeTree[] | SalesPoint[] | PosmProduct[] | WareHouse[]) {
    let allSelect = (items) => {
      items.forEach(element => {
        element["selectedAllGroup"] = "selectedAllGroup";
      });
    };

    allSelect(items);
  }
}
