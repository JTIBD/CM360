import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { UserService } from 'src/app/Shared/Services/Users';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';

@Component({
  selector: 'app-modal-excel-import-cm-user',
  templateUrl: './modal-excel-import-cm-user.component.html',
  styleUrls: ['./modal-excel-import-cm-user.component.css']
})
export class ModalExcelImportCmUserComponent implements OnInit {

  isFormValid: boolean = false;
  selectedFile: File = null;
	selectedFileName: string;
  tosterMsgError: string = "Something went wrong";
  fileError: string = '';
  errors:any = [];

  constructor(public activeModal: NgbActiveModal,
    private userService: UserService, private alertService: AlertService) {
      console.log(this.errors);
     }

  ngOnInit() {
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

  submit() {
    if(!this.selectedFile) {
      console.log('File is not selected');
      return;
    }

    const model = {'excelFile': this.selectedFile};
    console.log(model);
    this.userService.excelImportUser(model).subscribe(
      (res: any) => {
        if (res.data && res.data.item1) {
          this.alertService.tosterSuccess(`HHT User has been imported successfully`);
          this.activeModal.close(`HHT User has been imported successfully`);
        }
        else {
          var errors = res.data.item2; 
          this.errors = errors;
        }
      
      },
      (err) => {
        console.log(err);
        this.showError();
      }
    );
  }

  importAgain(){ 
    this.errors = [];
  }

  showError(msg: string = null) {
    this.activeModal.close(msg ? msg : this.tosterMsgError);
  }

}
