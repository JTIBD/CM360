import { DailyPOSM, DailyAudit } from '.';
import { User, UserInfo } from '../Users';
import { Outlet } from '../Sales';

export class DailyCMActivity{
    public id:number;
    // public fmId:number;
    public routeId:number;
    public outletId:number;
    // public outlets:any;
    // public cm:any;
    public cmId:number;
    public date:Date;
    public dateStr: string;
    public assignedFMUserId:number;
    public isAudit:boolean;
    public isSurvey:boolean;
    public isPOSM:boolean;
    public isConsumerSurveyActive:boolean;
    public status: number;
    public dailyPOSM:DailyPOSM=new DailyPOSM();
    public dailyAudit:DailyAudit=new DailyAudit();
    // public surveyQuestions: SurveyQuestion[]=[];
    public surveys: any[]=[];
    public consumerSurveys: any[]=[];
    public cm : User = new User();
    public assignedFMUser : UserInfo = new UserInfo();
    public outlet: Outlet  = new Outlet();

    //for display
    public fmUserName: string;
    public routeName: string;
		public outletName: string;
		public salesPointName: string;
    public cmUserName: string;
    public displayDate: string;
    public displayIsPOSM: string;
    public displayIsAudit: string;
    public displayIsSurvey: string;
    public displayIsConsSurAct: string;
    public displayDetails: string;
    public displayStatus: string;

    // for dashboard
    public completedPercentage: string;
    public  posmPercentage: string;
    public  surveyPercentage: string;
    public  auditPercentage: string;
  }


  export class SurveyQuestion{
   public id:number;
   public answer: string;
   public dailyActivityId: number;
   public questionId: number;
   public surveyId:number;
  }

