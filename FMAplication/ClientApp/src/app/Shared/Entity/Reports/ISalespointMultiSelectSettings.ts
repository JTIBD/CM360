import { PosmProduct } from 'src/app/Shared/Entity';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { NodeTree, SalesPoint } from '../Sales';
import { WareHouse } from '../Inventory';

export interface ISalesPointMultiSelectSettings{
    enableRegions: boolean | false;
    enableTerritories: boolean | false;
    enableAreas: boolean | false;
    enableSalespoint: boolean | false;
    enableDateRangeFilter: boolean | false;
    enablePosmItems: boolean | false;
    enableWarehouses: boolean | false;
    checkboxConfig?:{
      label:string;
    }
}

export class SalesPointMultiSelectItem {
  selectedRegions: NodeTree[] = [];
  selectedAreas: NodeTree[] = [];
  selectedTerritories: NodeTree[] = [];
  selectedSalesPoints: SalesPoint[] = [];
  selectedFromDate: NgbDateStruct;
  selectedToDate: NgbDateStruct;
  selectedPosmProducts: PosmProduct[] = [];
  selectedWarehouses: WareHouse[] = [];
}

export class SalesPointMultiSelectItemValues {
  selectedRegions: number[] = [];
  selectedAreas: number[] = [];
  selectedTerritories: number[] = [];
  selectedSalesPoints: number[] = [];
  selectedFromDate: string;
  selectedToDate: string;
  selectedPosmProducts: number[] = [];
  selectedWarehouses: number[] = [];
  isCheckboxSelected:boolean;
}