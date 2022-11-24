import { Component, OnInit } from '@angular/core';
import { settings } from 'cluster';
import { NodeHieararchy, NodeTree, SalesPoint } from 'src/app/Shared/Entity';
import { GetExecutionReport } from 'src/app/Shared/Entity/Reports';
import { ISalesPointMultiSelectSettings, SalesPointMultiSelectItem, SalesPointMultiSelectItemValues } from 'src/app/Shared/Entity/Reports/ISalespointMultiSelectSettings';
import { ExecutionReportField } from 'src/app/Shared/Enums/ExecutionReportField';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { ExcelService } from 'src/app/Shared/Modules/p-table/service/excel.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { CmTaskGenerationService } from 'src/app/Shared/Services/DailyActivity/cm-task-generation.service';
import { Utility } from 'src/app/Shared/utility';
import { ReportUtility } from 'src/app/Shared/Utility/reportUtility';

@Component({
  selector: 'app-execution-report',
  templateUrl: './execution-report.component.html',
  styleUrls: ['./execution-report.component.css']
})
export class ExecutionReportComponent implements OnInit {

  reportName: string = 'Execution Report';
  salesMultiSelectSettings : ISalesPointMultiSelectSettings = {
    enableRegions: false,
    enableTerritories: false,
    enableAreas: false,
    enableSalespoint: false,
    enableDateRangeFilter: true,
    enablePosmItems: false,
    enableWarehouses: false,
    checkboxConfig:{
      label:"Include outletwise"
    }
  };

  nodeHieararchies:NodeHieararchy;

  constructor(private cmTaskGenerationService:CmTaskGenerationService,
    private commonService:CommonService,
    private excelService:ExcelService,
    private alertService:AlertService) { }

  ngOnInit() {
  }

  createRequestModelFromSelection(selection:SalesPointMultiSelectItemValues){
    let model = new GetExecutionReport();
    model.fromDateTime = selection.selectedFromDate;
    model.toDateTime = selection.selectedToDate;
    return model;
  }

  getOutletWiseReport(selection: SalesPointMultiSelectItemValues){
    console.log(selection);
    let payload = this.createRequestModelFromSelection(selection);
    payload.salesPointIds = this.getSalesPointIdsFromSelection(selection);
    this.cmTaskGenerationService.getExecutionReportOutletWise(payload)
    .toPromise().then((data) => {
      if(!data.length){
        this.alertService.tosterInfo("No data found.");
        return;
      }
      let keys = Object.keys(data[0]);
      let totalColumn = ReportUtility.TotalColumnForOutletWiseExecutionReport;
      let sumFields = [
        totalColumn,
          ...keys.slice(keys.findIndex(x=>x === totalColumn)+1)
      ];
        let json:object[] = ReportUtility.MapExecutionResponse(data,sumFields);             
        const fileName = this.reportName +`_${Utility.getDateTimeSuffixForExcelFilename()}.xlsx`;
          this.excelService.exportToExcel(json, fileName);
      });
  }

  getSpWiseReport(selection: SalesPointMultiSelectItemValues){
    console.log(selection);
    let payload = this.createRequestModelFromSelection(selection);
    if(!selection.selectedSalesPoints.length){
      payload.salesPointIds = this.nodeHieararchies.salespoints.map(x=>x.salesPointId);
    }
    else payload.salesPointIds = selection.selectedSalesPoints;
    this.cmTaskGenerationService.getExecutionReportSalesPointWise(payload)
    .toPromise().then((data) => {
      if(!data.length){
        this.alertService.tosterInfo("No data found.");
        return;
      }
      let keys = Object.keys(data[0]);
      let totalColumn = ReportUtility.TotalColumnForSpWiseExecutionReport;
      let sumFields = [
          totalColumn,
          ...keys.slice(keys.findIndex(x=>x === totalColumn)+1)
      ];
        let json:object[] = ReportUtility.MapExecutionResponse(data,sumFields);                       
        const fileName = this.reportName +`_${Utility.getDateTimeSuffixForExcelFilename()}.xlsx`;
          this.excelService.exportToExcel(json, fileName);
      });
  }

  getTeritoryWiseReport(selection: SalesPointMultiSelectItemValues){
    console.log(selection);
    let payload = this.createRequestModelFromSelection(selection);
    payload.salesPointIds = this.getSalesPointIdsFromSelection(selection);        
    this.cmTaskGenerationService.getExecutionReportTeritoryWise(payload)
    .toPromise().then((data) => {
      if(!data.length){
        this.alertService.tosterInfo("No data found.");
        return;
      }
      let keys = Object.keys(data[0]);
      let totalColumn = ReportUtility.TotalColumnForTeritoryWiseExecutionReport;
      let sumFields = [
          totalColumn,
          ...keys.slice(keys.findIndex(x=>x === totalColumn)+1)
      ];
        let json:object[] = ReportUtility.MapExecutionResponse(data,sumFields);                       
        const fileName = this.reportName +`_${Utility.getDateTimeSuffixForExcelFilename()}.xlsx`;
          this.excelService.exportToExcel(json, fileName);
      });
  }

  getAreaWiseReport(selection: SalesPointMultiSelectItemValues){
    console.log(selection);
    let payload = this.createRequestModelFromSelection(selection);
    payload.salesPointIds = this.getSalesPointIdsFromSelection(selection);
    this.cmTaskGenerationService.getExecutionReportAreaWise(payload)
    .toPromise().then((data) => {
      if(!data.length){
        this.alertService.tosterInfo("No data found.");
        return;
      }
      let keys = Object.keys(data[0]);
      let totalColumn = ReportUtility.TotalColumnForAreaWiseExecutionReport;
      let sumFields = [
          totalColumn,
          ...keys.slice(keys.findIndex(x=>x === totalColumn)+1)
      ];
        let json:object[] = ReportUtility.MapExecutionResponse(data,sumFields);                       
        const fileName = this.reportName +`_${Utility.getDateTimeSuffixForExcelFilename()}.xlsx`;
          this.excelService.exportToExcel(json, fileName);
      });
  }

  getRegionWiseReport(selection: SalesPointMultiSelectItemValues){
    console.log(selection);
    let payload = this.createRequestModelFromSelection(selection);
    payload.salesPointIds = this.getSalesPointIdsFromSelection(selection);
    this.cmTaskGenerationService.getExecutionReportRegionWise(payload)
    .toPromise().then((data) => {
      if(!data.length){
        this.alertService.tosterInfo("No data found.");
        return;
      }
      let keys = Object.keys(data[0]);
      let totalColumn = ReportUtility.TotalColumnForRegionWiseExecutionReport;
      let sumFields = [
          totalColumn,
          ...keys.slice(keys.findIndex(x=>x === totalColumn)+1)
      ];
        let json:object[] = ReportUtility.MapExecutionResponse(data,sumFields);                       
        const fileName = this.reportName +`_${Utility.getDateTimeSuffixForExcelFilename()}.xlsx`;
          this.excelService.exportToExcel(json, fileName);
      });
  }

  getNationalReport(selection: SalesPointMultiSelectItemValues){
    console.log(selection);
    let payload = this.createRequestModelFromSelection(selection);
    payload.salesPointIds = this.getSalesPointIdsFromSelection(selection);
    this.cmTaskGenerationService.getExecutionReportNational(payload)
    .toPromise().then((data) => {
      if(!data.length){
        this.alertService.tosterInfo("No data found.");
        return;
      }
      let keys = Object.keys(data[0]);
      let totalColumn = ReportUtility.TotalColumnForNationalExecutionReport;
      let sumFields = [
          totalColumn,
          ...keys.slice(keys.findIndex(x=>x === totalColumn)+1)
      ];
        let json:object[] = ReportUtility.MapExecutionResponse(data,sumFields);                       
        const fileName = this.reportName +`_${Utility.getDateTimeSuffixForExcelFilename()}.xlsx`;
          this.excelService.exportToExcel(json, fileName);
      });
  }

  exportReport(selection: SalesPointMultiSelectItemValues){
    if(selection.isCheckboxSelected){
      this.getOutletWiseReport(selection);      
    }else{
      if(selection.selectedSalesPoints.length) this.getSpWiseReport(selection);
      else if(selection.selectedTerritories.length) this.getTeritoryWiseReport(selection);
      else if(selection.selectedAreas.length) this.getAreaWiseReport(selection);
      else if(selection.selectedRegions.length) this.getRegionWiseReport(selection);
      else{
        if(this.nodeHieararchies.regions.length) {
           if(this.nodeHieararchies.topLavelHierarchyCode == "N") this.getNationalReport(selection);            
           else this.getRegionWiseReport(selection);
        }
        else if(this.nodeHieararchies.areas.length) this.getAreaWiseReport(selection);
        else if(this.nodeHieararchies.teritories.length) this.getTeritoryWiseReport(selection);
        else this.getSpWiseReport(selection);
        
      }
    }

    
  }

  getSelectedRegionIdsFromSelection(selection:SalesPointMultiSelectItemValues){
    let regionIds = selection.selectedRegions;
    if(regionIds.length) return regionIds;        
    return this.nodeHieararchies.regions.map(x=>x.node.nodeId);
  }

  getSelectedAreaIdsFromSelection(selection:SalesPointMultiSelectItemValues){
    let areaIds = selection.selectedAreas;
    if(areaIds.length) return areaIds;
    else if(this.nodeHieararchies.regions.length){
      var regionIds = this.getSelectedRegionIdsFromSelection(selection);
      //@ts-ignore
      let regions:NodeTree[] = this.nodeHieararchies.regions.filter(x=>regionIds.includes(x.node.nodeId)).map(x=>x.nodes).filter(x=>!!x).flat();
      return regions.map(x=>x.node.nodeId);
    }
    return this.nodeHieararchies.areas.map(x=>x.node.nodeId);
  }

  getSelectedTerritoryIdsFromSelection(selection: SalesPointMultiSelectItemValues){
    let teritoryIds = selection.selectedTerritories;
    if(teritoryIds.length) return teritoryIds;
    else if(this.nodeHieararchies.areas.length){
      var areaIds = this.getSelectedAreaIdsFromSelection(selection);
      //@ts-ignore
      let teritories:NodeTree[] = this.nodeHieararchies.areas.filter(x=>areaIds.includes(x.node.nodeId)).map(x=>x.nodes).filter(x=>!!x).flat();
      return teritories.map(x=>x.node.nodeId);
    }
    return this.nodeHieararchies.teritories.map(x=>x.node.nodeId);
  }

  getSalesPointIdsFromSelection(selection: SalesPointMultiSelectItemValues){
    if(selection.selectedSalesPoints.length) return selection.selectedSalesPoints;
    else{
      let teritoryIds = this.getSelectedTerritoryIdsFromSelection(selection);
      //@ts-ignore
      let salesPoints:SalesPoint[] = this.nodeHieararchies.teritories.filter(x=> teritoryIds.includes(x.node.nodeId)).map(x=>x.salesPoints).filter(x=>!!x).flat();
      return salesPoints.map(x=>x.salesPointId);
    }
  }

  setMultiSelectSettings(){
    if(this.nodeHieararchies.regions.length) this.salesMultiSelectSettings.enableRegions = true;
    if(this.nodeHieararchies.areas.length) this.salesMultiSelectSettings.enableAreas = true;
    if(this.nodeHieararchies.teritories.length) this.salesMultiSelectSettings.enableTerritories = true;
    if(this.nodeHieararchies.salespoints.length) this.salesMultiSelectSettings.enableSalespoint = true;
  }

  saveNodeList(nodeHieararchies:NodeHieararchy){
    console.log(nodeHieararchies);
    this.nodeHieararchies = nodeHieararchies;
    this.setMultiSelectSettings();
  }

}
