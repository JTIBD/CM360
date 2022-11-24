import { CommonService } from './../../../Shared/Services/Common/common.service';
import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/Shared/Entity/Users/user';
import { Router } from '@angular/router';
import { UserService } from 'src/app/Shared/Services/Users';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { NgbCalendar, NgbDate, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { UserType } from 'src/app/Shared/Enums/CMUserType';



@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html',
    styleUrls: ['./create-user.component.css'],
    providers: [UserService]
})
export class CreateCmUserComponent implements OnInit {
    public selectedDate: NgbDateStruct;
    fromDate: NgbDate | null;

    public userModel: User = new User();
    // activeStatus: boolean;
    Users: any;
    salesPoints: any;
    enumStatusTypes: MapObject[] = StatusTypes.statusType;
    UserTypes: MapObject[] = UserType.UserTypes;
    
    passwordTextType = false;
    confirmPasswordTextType = false;
    noSpacePattern="/^[a-zA-Z0-9]*$/";
    phoneNumberPattern = "^[0-9]{11,11}$";

    constructor(private userService: UserService, private router: Router, 
        private calendar: NgbCalendar, private common:CommonService) { 

            
    }

    ngOnInit() {
        this.getAllUserInfoId();
        this.getAllSalesPoints();
        // this.userModel.isActive = "1";
  }

    submitUserForm(model) {
        let nameObj = {
            id : 0,
            name: this.userModel.name,
            code: this.userModel.code,
            email: this.userModel.email,
            phoneNumber: this.userModel.phoneNumber,
            address: this.userModel.address,
            familyContactNo: this.userModel.familyContactNo,
            designation: this.userModel.designation,
            // isActive: this.activeStatus,
            status: this.userModel.status,
            passWord: this.userModel.passWord,
            confirmPassword:this.userModel.confirmPassword,
            SalesPointIds : this.userModel.salesPointIds.length > 0 ? this.userModel.salesPointIds : [this.userModel.salesPointIds],
            userType : this.userModel.userType, 
            joiningDate : this.common.ngbDateToDate(this.selectedDate),
            nidBirthCertificate : this.userModel.nidBirthCertificate, 
            altCode :this.userModel.altCode,
        }
        console.log(nameObj);

        var result = this.userService.createUser(nameObj).subscribe(res => {
            this.router.navigate(['/users/users-list/']);
        });
        

    }
    getAllUserInfoId() {
        this.userService.getAllUserInfo().subscribe((res:any) => {
            console.log(res);
            this.Users = res.data;
        });
    }

    getAllSalesPoints() {
        this.userService.getAllSalesPointByCurrentUser().subscribe((res: any) => {
            console.log(res);
            this.salesPoints = res.data;
        });
    }

    public backToUserList() {
        this.router.navigate(['/users/users-list']);
    }
    
    togglePasswordTextType(){
        this.passwordTextType = !this.passwordTextType;
    }
    
    toggleConfirmPasswordTextType(){
        this.confirmPasswordTextType = !this.confirmPasswordTextType;
    }

    userTypeChange(value){
        this.userService.getUserTypeCode(value).subscribe(data => {
            this.userModel.code = data.data;
        })
    }




}
