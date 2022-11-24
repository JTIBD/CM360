import { Component, OnInit, Directive, Input, ViewChild } from '@angular/core';
import { UserInfo } from '../../../Shared/Entity/Users/userInfo';
import { Router } from '@angular/router';
import { AzureadService } from '../../../Shared/Services/Azure/azuread.service';
import { UserService } from 'src/app/Shared/Services/Users/user.service';
import { SalesPoint } from '../../../Shared/Entity';
import { Node } from '../../../Shared/Entity';
import { SalesPointService } from '../../../Shared/Services/Sales';
import { NodeService } from "../../../Shared/Services/Sales/node.service";
import { RoleService } from '../../../Shared/Services/Users/role.service';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { Hierarchy } from 'src/app/Shared/Entity/Sales/hierarchy';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';

@Component({
    selector: 'app-user-info-insert',
    templateUrl: './user-info-insert.component.html',
    styleUrls: ['./user-info-insert.component.css'],
    providers: [UserService, SalesPointService]
})
export class UserInfoInsertComponent implements OnInit {
    public userInfoModel: UserInfo = new UserInfo();
    enumStatusTypes: MapObject[] = StatusTypes.statusType;
    adUser: any;
    azureUser: any[] = [];
    activeStatus: boolean;
    adUserList: any[] = [];
    //salesPoints: SalesPoint[] = [];
    nodes: Node[] = [];
    regions: Node[] = [];
    nationals: Node[] = [];
    areas: Node[] = [];
    territories: Node[] = [];
    selection: number;
    selectedRole = null;
    roleList: any[] = [];
    adError: string;   
    hierarchyType: Hierarchy[] = [];
    phoneNumberPattern = "^[0-9]{11,11}$";


    constructor(private userService: UserService,
        private router: Router,
        private azureAdService: AzureadService,
        private salesPointService: SalesPointService,
        private nodeService: NodeService,
        private roleService: RoleService,
        private alertService: AlertService) { }

    ngOnInit() {
        // this.findAllAdUser();
        //this.getAllSalesPoint();
        this.getRoles();
        this.getHierarchyType();
        this.getAllNodes();
        
    }


    getHierarchyType() {
        this.userService.getAllHierarchy().subscribe((result: any) => {
          this.hierarchyType = result.data;
    
        });
      }

    submitUserForm() {

        
        
        if(this.userInfoModel.hierarchyId == 1){
            this.userInfoModel.regionNodeIds = [];
            this.userInfoModel.areaNodeIds = [];
            this.userInfoModel.territoryNodeIds = [];
          }
          else if(this.userInfoModel.hierarchyId == 2)
          {
            this.userInfoModel.nationalNodeIds = [];
            this.userInfoModel.areaNodeIds = [];
            this.userInfoModel.territoryNodeIds = [];
          }
          else if(this.userInfoModel.hierarchyId == 3)
          {
            this.userInfoModel.nationalNodeIds = [];
            this.userInfoModel.regionNodeIds = [];
            this.userInfoModel.territoryNodeIds = [];
          }
          else if(this.userInfoModel.hierarchyId == 4)
          {
            this.userInfoModel.nationalNodeIds = [];
            this.userInfoModel.regionNodeIds = [];
            this.userInfoModel.areaNodeIds = [];
          }


        console.log(this.userInfoModel);

        var result = this.userService.createUserInfo(this.userInfoModel).subscribe(res => {
            this.router.navigate(["/users-info"]).then(() => {
                this.alertService.titleTosterSuccess("Record has been saved successfully.");
              });
        },
        (error) => {
            // debugger;
            this.displayError(error);
          }, () => this.alertService.fnLoading(false)
        
        
        );

    }




    displayError(errorDetails: any) {
        // this.alertService.fnLoading(false);
        console.log("error", errorDetails);
        let errList = errorDetails.error.errors;
        if (errList.length) {
          console.log("error", errList, errList[0].errorList[0]);
          this.alertService.tosterDanger(errList[0].errorList[0]);
        } else {
          this.alertService.tosterDanger(errorDetails.error.msg);
        }
      }

    getRoles() {
        this.roleService.getRoleList().subscribe(
            (res: any) => {               
                console.log("Roles: ", res.data);
                this.roleList = res.data.model;
                this.selectedRole = this.roleList.length ? this.roleList[0].id : 0;
                //   this.getMenus();
            },
            (err: any) => {
                console.log(err);
            }
        );
    }


    findUserbyEmail(email: string) {     

    }


    onUserInfoSearch(email: any) {
        if (email.indexOf('@corp.jti.com') > -1) {

        } 
        else {
            email = email + '@corp.jti.com';     
        }
        
        this.azureAdService.getUsers(email).subscribe((res: any) => {
            this.azureUser = res.value;            
            this.adUser = this.azureUser[0];

            if (this.adUser !== undefined) {
               
                        this.adError = "";
                        this.userInfoModel.name = this.adUser.displayName;
                        this.userInfoModel.phoneNumber = this.adUser.mobilePhone;
                        this.userInfoModel.designation = this.adUser.jobTitle;
                        this.userInfoModel.email = email;
                        this.userInfoModel.adGuid = this.adUser.id;
                        this.userInfoModel.employeeId = 'emp123';
                    }
                    else {
                        this.adError = "Ops! User not found!! Check email."
                    }
                },        
        
        error => {        
            this.adError = "Ops! User not found!! Check email."
        })
    }


    adUserTableSearch(searchVal: string) {

        for (var i = 0; i < this.azureUser.length; i++) {
            if (this.azureUser[i].userPrincipalName.toLowerCase() == searchVal.toLowerCase()) {
                this.adUser = this.azureUser[i];
                break;
            }

        }
        //this.adUserList = this.azureUser.filter(function (hero) {
        //    if (hero.displayName.toLowerCase().includes(searchVal.toLowerCase())) {
        //        return hero;
        //    } else { return "Not Found" }
        //}) || [];
        // this.adUserList = this.azureUser.filter((record: any) => { if (record.headerName.toLowerCase().includes(searchVal.toLowerCase())) { return true } else { return false } }) || [];
    }


    //getAllSalesPoint() {
    //    this.salesPointService.getAllSalesPoint().subscribe(res => {
    //        this.salesPoints = res.data;
    //    });
    //}

    getAllNodes() {
        this.nodeService.getNodeList().subscribe(res => {
            this.nodes = res.data;
           // console.log('getAllNodes this.nodes', this.nodes);
            this.nationals = this.nodes.filter(s => s.code.startsWith('N'));
            this.regions = this.nodes.filter(s => s.code.startsWith('R'));
            this.areas = this.nodes.filter(s => s.code.startsWith('A'));
            this.territories = this.nodes.filter(s => s.code.startsWith('T'));
           // console.log('regions console', this.regions);
        });
    }

    public val: any;
    public isNational: boolean;
    public isRegion: boolean;
    public isArea: boolean;
    public isTerritory: boolean;
    //public isSalesPoint: boolean;

    changeFnOptType(val) {
        console.log("Dropdown selection:", val);
        this.isNational = val === 1;
        this.isRegion = val === 2;
        this.isArea = val === 3;
        this.isTerritory = val === 4;
        //this.isSalesPoint = val.some(el => el === 5);
    }

    public fnRouteUserInfoList() {
        this.router.navigate(['/users-info/users-infolist']);
    }
}
