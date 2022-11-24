import { Component, OnInit } from '@angular/core';
import { ISalesPointMultiSelectSettings, SalesPointMultiSelectItem, SalesPointMultiSelectItemValues } from 'src/app/Shared/Entity/Reports/ISalespointMultiSelectSettings';
import { ReportService } from 'src/app/Shared/Services/Reports/report-service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { Utility } from 'src/app/Shared/utility';

@Component({
  selector: 'app-cw-stock-update-report',
  templateUrl: './cw-stock-update-report.component.html',
  styleUrls: ['./cw-stock-update-report.component.css']
})
export class CwStockUpdateReportComponent implements OnInit {

  reportName: string = 'CW Stock Update Report';
  salesMultiSelectSettings : ISalesPointMultiSelectSettings = {
    enableRegions: false,
    enableTerritories: false,
    enableAreas: false,
    enableSalespoint: false,
    enableDateRangeFilter: true,
    enablePosmItems: false,
    enableWarehouses: true
  };
  constructor(private reportService: ReportService, private commonService: CommonService) { }

  ngOnInit() {
  }

  
  exportReport(items: SalesPointMultiSelectItemValues){
    console.log(items);
    
    let fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    this.reportService.exportCWStockUpdateReportToExcel(
      items.selectedFromDate, items.selectedToDate, items.selectedWarehouses).toPromise().then((data) => {
        const fileName = this.reportName +`_${Utility.getDateTimeSuffixForExcelFilename()}.xlsx`;
          this.commonService.DownloadFile(data, fileName, fileType);
      });
  }
}
