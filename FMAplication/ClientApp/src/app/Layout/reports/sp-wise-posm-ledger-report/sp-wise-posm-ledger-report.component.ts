import { Component, OnInit } from '@angular/core';
import { ISalesPointMultiSelectSettings, SalesPointMultiSelectItem, SalesPointMultiSelectItemValues } from 'src/app/Shared/Entity/Reports/ISalespointMultiSelectSettings';
import { ReportService } from 'src/app/Shared/Services/Reports/report-service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { Utility } from 'src/app/Shared/utility';

@Component({
  selector: 'app-sp-wise-posm-ledger-report',
  templateUrl: './sp-wise-posm-ledger-report.component.html',
  styleUrls: ['./sp-wise-posm-ledger-report.component.css']
})
export class SpWisePosmLedgerReportComponent implements OnInit {

  reportName: string = 'Salespoint Wise POSM Ledger Report';
  salesMultiSelectSettings : ISalesPointMultiSelectSettings = {
    enableRegions: true,
    enableTerritories: true,
    enableAreas: true,
    enableSalespoint: true,
    enableDateRangeFilter: true,
    enablePosmItems: true,
    enableWarehouses: false
  };
  constructor(private reportService: ReportService, private commonService: CommonService) { }

  ngOnInit() {
  }

  
  exportReport(items: SalesPointMultiSelectItemValues){
    console.log(items);
    items.selectedToDate = Utility.getDate(new Date(items.selectedToDate)).toISOString();
    let fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    this.reportService.exportSPWisePosmLedgerReport(items.selectedFromDate,items.selectedToDate,
      items.selectedSalesPoints,items.selectedPosmProducts)
    .toPromise().then((data) => {
        const fileName = this.reportName +`_${Utility.getDateTimeSuffixForExcelFilename()}.xlsx`;
          this.commonService.DownloadFile(data, fileName, fileType);
      });
  }

}
