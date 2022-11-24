import { PosmAssignService } from './../../../../Shared/Services/DailyActivity/posm-assign.service';
import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbCalendar, NgbDate, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { IDateRange, IParsedExcel, IParseExcelModel } from 'src/app/Shared/interfaces';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { UserService } from 'src/app/Shared/Services/Users';
import { FileUtilityService } from 'src/app/Shared/Services/fileUtility/file-utility.service';

@Component({
  selector: 'app-modal-import-posm-assign',
  templateUrl: './modal-import-posm-assign.component.html',
  styleUrls: ['./modal-import-posm-assign.component.css']
})
export class ModalImportPosmAssignComponent implements OnInit {

  isFormValid: boolean = false;
  selectedFile: File = null;
	selectedFileName: string;
  tosterMsgError: string = "Something went wrong";
  fileError: string = '';
  errors:any = [];
  excelData:IParsedExcel;



  includeFriday:boolean;
  public selectedFromDate: NgbDateStruct;
  public selectedToDate: NgbDateStruct;
  fromMinDate: NgbDate | null;
  toMinDate: NgbDate | null;
  toMaxDate: NgbDate | null;
 


  salesPoints: any;
  selectedSalesPointId;

  constructor(public activeModal: NgbActiveModal,
    private userService: UserService, 
    private alertService: AlertService, private PosmAssignService: PosmAssignService,
    private commonService:CommonService, private calendar:NgbCalendar, 
    private fileService:FileUtilityService) {
      
      this.getAllSalesPoints();
      this.fromMinDate = calendar.getToday();
     }

  ngOnInit() {
  }

  getAllSalesPoints() {
    this.userService.getAllSalesPointByCurrentUser().subscribe((res: any) => {
        console.log(res);
        this.salesPoints = res.data;
       
    });
}

	onChangeInputFile(event: any) {
		if (event.target.files && event.target.files[0]) {
      const file = event.target.files[0];
      this.selectedFile = file;
			if (!this.isValidFile(file)) return;
      this.isFormValid = true;
      this.selectedFileName = file.name;
    } else {
      this.clearFile();
		}
	}

	isValidFile(file) {
		const fileExt = file.name.split('.').pop().toLowerCase();
		if (!(['xlsx'].indexOf(fileExt) > -1)) {
			this.fileError = 'Invalid file type';
      this.clearFile();
			return false;
		}
		this.fileError = '';
		return true;
	}
  
  clearFile() { 
    this.selectedFile = null;
    this.isFormValid = false;
    this.selectedFileName = '';
  }

//   if (file) {
//     let model: IParseExcelModel = {
//         file
//     }
//     this.fileUtilityService.ParseExcel(model).subscribe((data) => {
//         this.excelData = data;
//         this.createTransactions(data);              
//     });
// }

  submit() {
    if(!this.selectedFile) {
      console.log('File is not selected');
      return;
    }

  const model = {'excelFile': this.selectedFile};
  this.PosmAssignService.excelImportPosmAssign(model).subscribe(
    (res: any) => {
      console.log(res);
     
      if (res.data && res.data.item1) {
        this.alertService.tosterSuccess(`Posm Assign has been imported successfully`);
        this.activeModal.close(`Posm Assign has been imported successfully`);
      }
      else {
        var errors = res.data.item2; 
        this.errors = errors;
      }

    },
    (err) => {
      console.log(err);
      this.activeModal.close(`failed`);
      this.displayError(err);
    }
  );

}

  importAgain(){ 
    this.errors = [];
  }

  private displayError(errorDetails: any) {
    console.log("error", errorDetails);
    let errList = errorDetails.error;
    if (errList.length) {
        console.log("error", errList, errList[0].errorList[0]);
        this.alertService.tosterDanger(errList[0].errorList[0]);
    } else {
      let message = errorDetails.error.Message;
      if (message && message.length > 0) {

        setTimeout(() => {
          this.alertService.alert(message);
      }, 1010);
        
      }
         
    } 
}

  handleFromDateChange(){   
    this.setFromToDateMinMax();
  }

  addDays(date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
  }

  downloadExcel() { 
    const model = {
      from : this.commonService.ngbDateToDate(this.selectedFromDate).toISOString(), 
      to : this.commonService.ngbDateToDate(this.selectedToDate).toISOString(), 
      excludeFriday:  !this.includeFriday, 
      salesPointId : this.selectedSalesPointId
    }
    let fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    this.PosmAssignService.downloadPosmAssignExcelFile(model).toPromise().then(data => {
      var headers = data.headers.get('content-disposition');
      var fileName = headers.split('filename=')[1].split(';')[0];
      fileName = fileName.replace(/^"|"$/g, '');
      this.commonService.DownloadFile(data.body, fileName, fileType);
    })
  }

  checkValue(value){
    
    this.setFromToDateMinMax();
  }

  

  private setFromToDateMinMax() {
    let toMinDate = this.addDays(this.commonService.ngbDateToDate(this.selectedFromDate), 0);
    let toMaxDate = this.addDays(this.commonService.ngbDateToDate(this.selectedFromDate), this.includeFriday ? 6 : 7);

    let toMinDateStruct = this.commonService.dateToNgbDate(toMinDate);
    let toMaxDateStruct = this.commonService.dateToNgbDate(toMaxDate);

    this.toMinDate = new NgbDate(toMinDateStruct.year, toMinDateStruct.month, toMinDateStruct.day);
    this.toMaxDate = new NgbDate(toMaxDateStruct.year, toMaxDateStruct.month, toMaxDateStruct.day);
    this.selectedToDate = null;
  }
}
