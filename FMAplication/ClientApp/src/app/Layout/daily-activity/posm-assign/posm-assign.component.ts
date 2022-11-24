import { IDropdown } from './../../../Shared/interfaces/IDropdown';
import { UserService } from 'src/app/Shared/Services/Users';
import { ModalImportPosmAssignComponent } from './modal-import-posm-assign/modal-import-posm-assign.component';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { PosmAssignService } from './../../../Shared/Services/DailyActivity/posm-assign.service';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { PosmAssign, PosmAssignDetails, PosmAssignDetailsTable, PosmAssignTableName, PosmParams, TaskStatus } from 'src/app/Shared/Entity/PosmAssign/posmAssign';
import { Utility } from 'src/app/Shared/utility';
import { IDateRange, IPaginator } from 'src/app/Shared/interfaces';
import { Paginator } from 'primeng/paginator';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
@Component({
  selector: 'app-posm-assign',
  templateUrl: './posm-assign.component.html',
  styleUrls: ['./posm-assign.component.css']
})
export class PosmAssignComponent implements OnInit {


 ListView = "ListView";
 DetailsView = "DetailsView";
 currentView :string = "";



  closeResult: string;
  PosmAssigns: PosmAssignTableName[] = [];
  PosmAssignDetails: PosmAssignDetailsTable[] = [];
  PosmAssignList: PosmAssign[] = [];
  PosmAssignDetailsList: PosmAssignDetails[] = [];
  params = new PosmParams();
  totalNumber: number;

  salesPointDropdownData : IDropdown[] = [];

 

  @ViewChild("paging", { static: false }) paging: Paginator;


  public ptableSettings: IPTableSetting ;
  public posmDetailstableSettings: IPTableSetting ;

  permissionGroup: PermissionGroup = new PermissionGroup();
  private initPermissionGroup() {
    this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
  }

  constructor(
    private router: Router,
    private modalService: NgbModal,
    private activatedRoute: ActivatedRoute,
    private alertService: AlertService, 
    private PosmAssignService: PosmAssignService, 
    private userService: UserService,
    private activityPermissionService: ActivityPermissionService) {
      this.currentView = this.ListView; 
      this.getPosmAssigns();
      this.getSalesPoints();
    }

  ngOnInit() {
  }

  ngAfterViewInit(){
       this.enableCurrentPageReport();
  }

  enableCurrentPageReport(){
    let timer = setInterval(()=>{
      if(this.paging){
        this.paging.showCurrentPageReport=true;
        clearInterval(timer);
      }
    },1); 
  }

  getPaginationStatus(){    
    return Utility.getPaginationStatus(this.totalNumber,this.params.pageIndex,this.params.pageSize,this.PosmAssigns.length);    
  }


  getPosmAssigns(){ 
    this.PosmAssignService.getPosmAssigns(this.params).subscribe((data:IPaginator<PosmAssign>) => {
      this.PosmAssignList = data.data;
       this.dataToMapPosmTableData(this.PosmAssignList);
       this.totalNumber = data.count;
    })
  }


  getSalesPoints() {
    this.userService.getAllSalesPointByCurrentUser().subscribe((res: any) => {
      const defaultValue = [{name : "All SalesPoint", id : 0}];
    const salesPoints =  [...defaultValue,...res.data];
     this.mapToDropDown(salesPoints);
     this.initialize();
     this.initPermissionGroup();
    });
  }

  mapToDropDown(data:any[]) {
    this.salesPointDropdownData = data.map(x => {
      const dropdownData :IDropdown = {
        label : x.name, 
        value : x.id
      };
      return dropdownData;
    });
  }
  dataToMapPosmTableData(data:PosmAssign[]) {
    this.PosmAssigns = data.map(x => {
      const data = new PosmAssignTableName();
      data.id = x.id;
      data.salesPointName = x.salesPoint.code;
      data.salesPointId = x.salesPointId,
      data.cmUser = `${x.cmUser.name} (${x.cmUser.code})`;
      data.cmUserId = x.cmUserId,
      data.lines = x.lines;
      data.date = Utility.getDateToStringFormat(x.dateStr);
      
      data.sumOfQuantity = x.sumOfQuantity;
      data.status = TaskStatus[x.taskStatus] 
      return data;
    });
  }

  initialize() {
    this.ptableSettings =  {
      tableID: "posmAssignPtable",
      tableClass: "table table-border ",
      tableName: "",
      tableRowIDInternalName: "id",
      tableColDef: [
        {
          headerName: "Sales Point",
          width: "20%",
          internalName: "salesPointName",
          sort: true,
          type: "",
        },
        {
          headerName: "User",
          width: "25%",
          internalName: "cmUser",
          sort: true,
          type: "",
        },
        {
          headerName: "Date",
          width: "15%",
          internalName: "date",
          sort: true,
          type: "",
        },
        {
          headerName: "#Lines ",
          width: "15%",
          internalName: "lines",
          sort: true,
          type: "",
        },
        {
          headerName: "Quantity",
          width: "15%",
          internalName: "sumOfQuantity",
          sort: true,
          type: "",
        },
        {
          headerName: "Status",
          width: "20%",
          internalName: "status",
          sort: true,
          type: "",
        }
      ],
  
      enabledSearch: true,
      enabledSerialNo: true,
      pageSize: this.params.pageSize,
      enabledPagination: true,
      //enabledAutoScrolled:true,
      //enabledCellClick: true,
      enabledColumnFilter: true,
      enabledDeleteBtn: false,
      tableFooterVisibility: false,
      enabledViewBtn: true,
      enabledServerSitePaggination: true,
      enablePazeSizeSelection: true,
      enabledDataLength: true, 
      enableDropdownFilter:true,
      dropdownData:  this.salesPointDropdownData, 
      selectedDropdownValue : this.salesPointDropdownData.length > 0 ? this.salesPointDropdownData[0].value : 0, 
      dropdownLabel: "SalesPoint", 
      enableDateRangeFilter:true,
      intialDateRange:{
        from:this.params.fromDate,
        to:this.params.toDate,
      },
    };



    this.posmDetailstableSettings =  {
      tableID: "posmAssignDetailsPtable",
      tableClass: "table table-border ",
      tableName: "",
      tableRowIDInternalName: "id",
      tableColDef: [
        {
          headerName: "POSM Code",
          width: "35%",
          internalName: "posmProductCode",
          sort: true,
          type: "",
        },
        {
          headerName: "POSM Name",
          width: "35%",
          internalName: "posmProductName",
          sort: true,
          type: "",
        },
        {
          headerName: "Quantity",
          width: "30%",
          internalName: "quantity",
          sort: true,
          type: "",
        },
      ],
  
      enabledSearch: true,
      enabledSerialNo: true,
      pageSize: this.params.pageSize,
      enabledPagination: true,
      //enabledAutoScrolled:true,
      //enabledCellClick: true,
      enabledColumnFilter: true,
      tableFooterVisibility: false
    };
  }


  openExcelImportModal() {
    let ngbModalOptions: NgbModalOptions = {
      backdrop: 'static',
      keyboard: false
    };
    const modalRef = this.modalService.open(ModalImportPosmAssignComponent, ngbModalOptions);

    modalRef.result.then((result) => {
      console.log(result);
      this.closeResult = `Closed with: ${result}`;
      this.router.navigate(['/task/posm-assign/']);
      this.getPosmAssigns();
    },
    (reason) => {
        console.log(reason);
    });
  }

  paginate(event) {
    this.params.pageIndex = Number(event.page) + 1;
    this.getPosmAssigns();
    this.ptableSettings.serverSitePageIndex = this.params.pageIndex;
  }




  reset() {
    this.paging.changePage(0);
    this.params.pageIndex = 1;
  }

  fnSearch($event: any) {
    
    this.params.search = $event.searchVal;
    this.paging.changePage(0);
  }

  handlePazeSizeChange(pageSize: number) {
    //this.reset();
   
    this.params.pageSize = pageSize;
    this.paging.changePage(0);
  }

  
  dropDownChange(selected:string){
     this.params.salesPointId = selected != null ? Number.parseInt(selected) : 0;
    this.getPosmAssigns();
    
     
  }
  handleDateRange(dateRange:IDateRange) {
    this.params.fromDate = dateRange.from;
    this.params.toDate = dateRange.to;
    this.paging.changePage(0);
  }

  changeView(viewName){
    this.alertService.fnLoading(true);
    this.currentView = viewName;
    this.alertService.fnLoading(false);
  }
  haneleCustomActivityOnRecord({ action: action, record: recordInfo }) {
    console.log(recordInfo);
    if (action === "view-item") {
        var model = {
          cmUserId: recordInfo.cmUserId,
          salesPointId: recordInfo.salesPointId, 
          date : Utility.stringToDate(recordInfo.date).toISOString()
        }
        this.PosmAssignService.getPosmAssignDetails(model).subscribe((data:PosmAssignDetails[]) => {
         this.PosmAssignDetailsList = data
         this.mapPosmAssignDetails()
         this.changeView(this.DetailsView);
        })
    }
  }

  mapPosmAssignDetails(){ 
    this.PosmAssignDetails = this.PosmAssignDetailsList.map(x => {
      const data = new PosmAssignDetailsTable();
   
      data.quantity = x.quantity;
      data.posmProductCode = x.posmProduct.code, 
      data.posmProductName = x.posmProduct.name
      return data;
    });
  }

  fnTransactionPtableCellClick(event) {
    if (event.cellName !== "details") return;
    


  }







  


}
