import { Status } from '../../Enums/status';
import { CMUserType } from "../../Enums/CMUserType";

export class User {


    public userType:CMUserType;
    public id: number;
    public name: string;
    public altCode:string;
    public code: string;
    public email: string;
    public designation: string;
    public phoneNumber: string;
    public nidBirthCertificate: string;
    public joiningDate: Date;
    public passWord: string;
    public address: string;
    public isActive?: string;
    public status: number;
    public familyContactNo: string;
    public fmUserId: number;
    public confirmPassword: string;
    public salesPointIds: number[];
    public salesPointId : number;

    

    constructor() {
      this.status = Status.Active;
    }

}