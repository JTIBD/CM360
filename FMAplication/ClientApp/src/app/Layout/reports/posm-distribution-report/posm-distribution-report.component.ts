import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { ReportService } from 'src/app/Shared/Services/Reports/report-service';
import { ISalesPointMultiSelectSettings, SalesPointMultiSelectItem, SalesPointMultiSelectItemValues } from './../../../Shared/Entity/Reports/ISalespointMultiSelectSettings';
import { NodeHieararchy } from './../../../Shared/Entity/Sales/nodeHiearchy';
import { Utility } from 'src/app/Shared/utility';
import { NodeTree } from './../../../Shared/Entity/Sales/nodeTree';
import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/Shared/Services/Users/user.service';
import { SalesPoint } from 'src/app/Shared/Entity';
import { IDropdown } from 'src/app/Shared/interfaces/IDropdown';

@Component({
  selector: 'app-posm-distribution-report',
  templateUrl: './posm-distribution-report.component.html',
  styleUrls: ['./posm-distribution-report.component.css']
})
export class PosmDistributionReportComponent implements OnInit {
  reportName: string = 'POSM Distribution Report';
  salesMultiSelectSettings : ISalesPointMultiSelectSettings = {
    enableRegions: true,
    enableTerritories: true,
    enableAreas: true,
    enableSalespoint: true,
    enableDateRangeFilter: true,
    enablePosmItems: false,
    enableWarehouses: false
  };

  constructor(private reportService: ReportService, private commonService: CommonService) { }

  ngOnInit() {
   
  }

  exportReport(items: SalesPointMultiSelectItemValues){
    console.log(items);
    
    let fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    this.reportService.exportCWDistributionReportToExcel(
      items.selectedFromDate, items.selectedToDate, items.selectedSalesPoints).toPromise().then((data) => {
        const fileName = this.reportName +`_${Utility.getDateTimeSuffixForExcelFilename()}.xlsx`;
          this.commonService.DownloadFile(data, fileName, fileType);
      });
  }


}
 //nodeTree -> node , nodes , salesPoint
      // With recursion

      // let fun = (tree: NodeTree) => {

      //   if(tree.nodes.length <= 0){
      //     tree.salesPoints.map(x => {
      //       this.salesPointsList.push(x);
      //     })
      //     //this.salesPointsList.push(tree.salesPoints);
      //   }
      //   tree.nodes.forEach(node => {
      //     fun(node);
      //   });
      // }

      // fun(this.nodeTree[0]);
      // console.log(this.salesPointsList);

      // let fun = (trees: NodeTree[]) => {
      //   trees.forEach(tr => {
      //     debugger;
      //     if (!!tr.nodes && tr.nodes.length) fun(tr.nodes);
      //     else if (!!tr.salesPoints && !!tr.salesPoints.length) {
      //       tr.salesPoints.forEach(sl => {

      //       })
      //     }
      //   });
      // }
      //fun(this.nodeTree);
      // salespoint , territory , area , region