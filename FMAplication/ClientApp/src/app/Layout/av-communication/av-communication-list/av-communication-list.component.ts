import { AlertService } from './../../../Shared/Modules/alert/alert.service';
import { AvCommunicationService } from './../../../Shared/Services/AvCommunication/avCommunication.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit, ViewChild } from '@angular/core';
import { Paginator } from 'primeng/paginator';
import { colDef, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { IDateRange } from 'src/app/Shared/interfaces';
import { AvCommunication, AvCommunicationTable, CampaignType } from 'src/app/Shared/Entity/AVCommunications/avCommunication';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';

@Component({
  selector: 'app-av-communication-list',
  templateUrl: './av-communication-list.component.html',
  styleUrls: ['./av-communication-list.component.css']
})
export class AvCommunicationListComponent implements OnInit {

  avCommunicationList:AvCommunication[]=[];
  avCommunicationTable:AvCommunicationTable[]=[];
  pageSize = 10;
  pageIndex = 0;

  @ViewChild("paging", { static: false }) paging: Paginator;
  showingPageDetails:any;

  public ptableSettings:IPTableSetting= {
    tableID: "AV-Communication-List-table2",
    tableClass: "table-responsive",
    tableName: 'AV Communication List',
    tableRowIDInternalName: "Id",
    tableColDef: [
      { headerName: 'Campaign Name', width: '15%', internalName: 'campaignName', sort: true, type: "" },
      { headerName: 'Campaign Type', width: '15%', internalName: 'campaignType', sort: true, type: "" },
      { headerName: 'Brand/SKU', width: '20%', internalName: 'brand', sort: true, type: "" },
      { headerName: 'Description', width: '40%', internalName: 'description', sort: true, type: "" },
      { headerName: 'File', width: '10%', internalName: 'downloadButtonText',sort: false, type: "button", onClick: 'true', innerBtnIcon:"fa fa-info" },
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: this.pageSize,
    enabledPagination: true,
    enabledEditBtn: true,
	  enabledDeleteBtn: true,
    enabledColumnFilter: true,
    enabledCellClick: true,
    enabledRecordCreateBtn: true,
    // enableDateRangeFilter:true,
    tableFooterVisibility:true,
    

  };

  private permissionGroup: PermissionGroup = new PermissionGroup();
  private initPermissionGroup() {
      this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
      this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
      this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
      this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;
  }

  constructor(private router:Router, private avService:AvCommunicationService, private alertService:AlertService,
    private activityPermissionService: ActivityPermissionService, private activatedRoute: ActivatedRoute) { 
    this.getAllAvCommunications();
    this.initPermissionGroup();
  }

  ngOnInit() {
  }

  getAllAvCommunications(){
    this.avService.getAll().subscribe((data:AvCommunication[]) => {
      this.avCommunicationList = data;
      this.mapToTable(this.avCommunicationList)
    })
  }

  mapToTable(data: AvCommunication[]) {
    this.avCommunicationTable = data.map(x => {
      const data = new AvCommunicationTable();
      data.id = x.id;
      data.campaignName = x.campaignName;
      data.brand = `${x.brandModel.code}-${x.brandModel.name}`;
      data.campaignType =  CampaignType[x.campaignType],
      data.description = x.description;
      data.filePath = x.filePath;
      data.disableEdit= !x.isEditable;
      data.disableDelete = !x.isDeletable;
      return data;
    });
  }


  haneleCustomActivityOnRecord({ action: action, record: recordInfo }){
    console.log(action,recordInfo);
    if(action === "view-item"){
      
    }
    if (action == "new-record") {
      this.router.navigate(['/av-communication/add-av-communication']);
    }
    else if (action == "edit-item") {
      this.router.navigate([`/av-communication/add-av-communication/${recordInfo.id}`]);
    }
    else if (action == "delete-item") {
      this.delete(recordInfo.id);
    }
  }

  delete(id: number) {
    this.alertService.confirm("Are you sure you want to delete this item?", () => {
      this.avService.delete(id).subscribe((res: any) => {
        console.log('res from del func', res);
        this.alertService.tosterSuccess("AV communication has been deleted successfully.");
        this.getAllAvCommunications();
      },
        (error) => {
          console.log(error);
        }
      );
    }, () => {

    });

  }

  
  reset(){
  
    this.pageIndex = 1;
    this.paging.changePage(0);
  }
  
  fnSearch($event: any) {
    this.reset();
  }

  handlePazeSizeChange(pageSize:number){
    console.log(pageSize);
    this.pageIndex=1;
    this.pageSize = pageSize;
    this.paging.changePage(0);   
  }
  paginate(event) {    
    this.pageIndex = Number(event.page) + 1;                    
   
  }

  fnPtableCellClick(event: any) {
    console.log("cell click: ", event);
   
    let link = document.createElement('a');
    link.href =  event.record.filePath; 
    link.target = "_blank";
    link.download = event.record.campaignName;
    link.click();
  }

}
