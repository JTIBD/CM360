import { IDateRange } from "./interfaces";
import { IPTableSetting } from "./Modules/p-table";
import * as moment from 'moment';
import { Node, NodeHieararchy, NodeTree, SalesPoint } from "./Entity";
import { IDropdown } from "./interfaces/IDropdown";

export class Utility {
    static MaximumWorkableInteger=999999999;
    static allSelectionValue=-1;
    static defaultPageSize = 10;
    static getDateToStringFormat(dateString:string) {
        var date = new Date(dateString);
        return `${('00' + date.getDate()).slice(-2)}-${('00' + (date.getMonth() + 1)).slice(-2)}-${date.getFullYear()}`;
    }

    static getPTableHtml(pTableSetting: IPTableSetting, pTableData: any[]){
      
    }

    static adjustDateRange(dateRanage:IDateRange){
      let fromDate = new Date(dateRanage.from);
      let toDate = new Date(dateRanage.to);
      if(fromDate > toDate) [fromDate,toDate]=[toDate,fromDate];
      dateRanage.from = new Date(fromDate.getFullYear(),fromDate.getMonth(),fromDate.getDate()).toISOString();
      dateRanage.to = new Date(toDate.getFullYear(),toDate.getMonth(),toDate.getDate(),23,59,59).toISOString();
    }

    static stringToDate(dateString:string){
      var date = new Date(dateString.replace( /(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"))
      return date;
    }
    static getLastDateOfCurrentMonth(){
      let date = new Date();
      return new Date(date.getFullYear(), date.getMonth()+1 , 0 ,23,59,59).toISOString();
    }
    static getFirstDateOfCurrentMonth(){
      let date = new Date();
      return new Date(date.getFullYear(), date.getMonth() , 1).toISOString();
    }
    static isInt(value:any) {
      var x = parseFloat(value);
      return !isNaN(value) && (x | 0) === x;
    }
    static clone(obj:object){
      return JSON.parse(JSON.stringify(obj));
    }
    static getDateTimeSuffixForExcelFilename(){
      return moment().format("D MMMM YYYY-HHmm");
    }
    static getNodeHierarchyFromNodeTree(nodeTree:NodeTree[]){
      let list:object[][] = [];

      let fun = (tree:NodeTree[])=>{        
        list.push(tree);        
        //@ts-ignore
        let salesPoints = tree.filter(x=>!!x.salesPoints && x.salesPoints.length).map(x=>x.salesPoints).flat();
        if(!!salesPoints.length){
          list.push(salesPoints);
        }
        else {
          //@ts-ignore
          let childNodeTrees = tree.map(x=>x.nodes).flat();
          fun(childNodeTrees);
        }
      }
      fun(nodeTree);
      
      const nodeHierarchy = new NodeHieararchy();
      nodeHierarchy.topLavelHierarchyCode = nodeTree[0].hierarchyCode;      
      nodeHierarchy.salespoints = list.pop() as SalesPoint[];
      if(!!list.length) nodeHierarchy.teritories = list.pop() as NodeTree[];
      if(!!list.length) nodeHierarchy.areas = list.pop() as NodeTree[];
      if(!!list.length) nodeHierarchy.regions = list.pop() as NodeTree[];

      return nodeHierarchy;

    }

    static handledAllSelection(newSelections:IDropdown[],existingSelectedValues:number[],dropdownOptions : IDropdown[]){
      const isAllSelected = newSelections.some(x=>x.value == Utility.allSelectionValue);
      if(isAllSelected !== existingSelectedValues.includes(Utility.allSelectionValue)){
        if(isAllSelected) return dropdownOptions.map(x=>x.value);
        else return [];
      }
      else if(newSelections.filter(x=>x.value !== Utility.allSelectionValue).length === dropdownOptions.length-1){
        return dropdownOptions.map(x=>x.value);        
      }
      else if(newSelections.length !== dropdownOptions.length && existingSelectedValues.length === dropdownOptions.length){
        return newSelections.filter(x=>x.value !== Utility.allSelectionValue).map(x=>x.value);        
      }  
      return undefined;
  
    }

    static getPaginationStatus(total:number,pageIndex:number,pageSize:number,itemCount){
      if(!total) total = 0;
      let start = (pageIndex-1)*pageSize + 1;
      let end = (pageIndex)*pageSize;
      if(total < end){
        end = total ;
      }
      if(end === 0) start = 0;
      const status = `Showing ${start} to ${end} of ${total} records`;
      return status;
    }
    static getSum(data:any[]){
      let sum = 0;
      data.forEach(x=>{
        let value = Number(x);
        if(isNaN(value)) value = 0;
        sum += value;
      });
      return sum;
    }

    static formateDigitCount(value:number, minimumDigitCount:number){
      return value.toLocaleString('en-US',{minimumIntegerDigits:2,useGrouping: false});
    }

    static getDate(dateTime:Date){
      return new Date(dateTime.getFullYear(),dateTime.getMonth(),dateTime.getDate());
    }
}