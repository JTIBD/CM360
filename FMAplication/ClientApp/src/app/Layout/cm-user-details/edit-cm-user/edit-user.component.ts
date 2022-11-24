import { Component, OnInit } from '@angular/core';
import { User } from '../../../Shared/Entity/Users/user';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { UserService } from 'src/app/Shared/Services/Users';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { CMUserType, UserType } from 'src/app/Shared/Enums/CMUserType';
import { NgbCalendar, NgbDate, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
    styleUrls: ['./edit-user.component.css'],
    providers: [UserService]
})
export class EditCmUserComponent implements OnInit {
    user: User = new User();
    Users: number[] = [];
    salesPoints: any;
    // activeStatus: boolean;
    enumStatusTypes: MapObject[] = StatusTypes.statusType;
    passwordTextType = false;
    confirmPasswordTextType = false;
    UserTypes: MapObject[] = UserType.UserTypes;

    noSpacePattern="[^\s-]";
    phoneNumberPattern = "^[0-9]{11,11}$";

    public selectedDate: NgbDateStruct;
    fromDate: NgbDate | null;
    
    constructor(
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private alertService: AlertService,
        private userService: UserService, 
        private calendar: NgbCalendar, private common:CommonService
    ) { 

    }

    ngOnInit() {
        this.getAllUserInfoId();
        this.getAllSalesPoints();

        if ((this.activatedRoute.snapshot.params).hasOwnProperty('id')) {
            this.getUserById(this.activatedRoute.snapshot.params.id);
        }
    }





    getUserById(id: number) {
        console.log(id);
        this.alertService.fnLoading(true);
        this.userService.getUserById(id).subscribe(
            (res: any) => {
                this.user = res.data || new User();
                if (this.user.userType === CMUserType.CMR){
                    this.user.salesPointId = this.user.salesPointIds[0];
                }
                this.selectedDate = this.common.dateToNgbDate(this.user.joiningDate) 
                this.user.passWord = res.data.password || '';
                this.user.confirmPassword = this.user.passWord;
                
                if (Object.keys(res.data).length == 0) {
                    this.showError("No such user!Create a new user");
                    this.router.navigate([`/users/users-list/`]);
                }
                else {
                    console.log("User ", this.user);
                    //this.router.navigate([`/users/users-list/`]);
                }
            },
            (error) => {
                console.log(error);
                this.showError(error.message);
                this.router.navigate([`/users/users-list/`]);
            },
            () => {
                this.alertService.fnLoading(false);
            }
        );
    }


    showError(msg: string) {
        this.alertService.fnLoading(false);
        this.alertService.tosterDanger(msg);

    }

    submitUserForm(model) {

        let nameObj = {
            id : this.user.id,
            name: this.user.name,
            code: this.user.code,
            email: this.user.email,
            designation: this.user.designation,
            phoneNumber: this.user.phoneNumber,
            address: this.user.address,
            familyContactNo: this.user.familyContactNo,
            status: this.user.status,
            passWord: this.user.passWord,
            confirmPassword:this.user.confirmPassword,
            SalesPointIds : this.user.userType === CMUserType.TMS  ? this.user.salesPointIds : [this.user.salesPointId],
            userType : this.user.userType, 
            joiningDate : this.common.ngbDateToDate(this.selectedDate),
            nidBirthCertificate : this.user.nidBirthCertificate, 
            altCode :this.user.altCode,
        }
        
        var result = this.userService.updateUser(nameObj).subscribe(res => {
            this.router.navigate(['/users/users-list/']);
        });


    }

    getAllUserInfoId() {
        this.userService.getAllUserInfo().subscribe((res: any) => {
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

}
