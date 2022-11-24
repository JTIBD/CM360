import { DailyBaseTask } from "../Entity/DailyTasks/dailyBaseTask";
import { BaseReportTableData } from "../Entity/Reports/baseReportTableData";
import { ReportConst } from "../Entity/Reports/ReportConst";
import { ExecutionReportField } from "../Enums/ExecutionReportField";
import { Utility } from "../utility";
import * as moment from "moment";

export class ReportUtility{
    static TotalColumnForOutletWiseExecutionReport = ExecutionReportField.TotalDuration;
    static TotalColumnForSpWiseExecutionReport = ExecutionReportField.CmrCode;
    static TotalColumnForTeritoryWiseExecutionReport = ExecutionReportField.SalesPoint;
    static TotalColumnForAreaWiseExecutionReport = ExecutionReportField.Territorry;
    static TotalColumnForRegionWiseExecutionReport = ExecutionReportField.Area;
    static TotalColumnForNationalExecutionReport = ExecutionReportField.Region;

    static SetOutletClosedStatus(row:BaseReportTableData){
        row.displayStatus = ReportConst.OutletClosedReason;
        row.reason = ReportConst.OutletClosedReason;
    }
    static SetIncompleteStatus(row:BaseReportTableData,dailyTask:DailyBaseTask){
        row.displayStatus = ReportConst.InCompleteReason;
        if(dailyTask.reason) row.reason = dailyTask.reason.reasonInEnglish;
    }

    private static AddSumObject(keys:string[],json:object[]){
        let sumObj ={};
        sumObj[keys[0]] = "Total";
        for (let index = 1; index < keys.length; index++) {
          sumObj[keys[index]] = Utility.getSum(json.map(j=>j[keys[index]]));          
        }
        json.push(sumObj);
    }

    static MapExecutionResponse(res:object[],sumFields:string[]){
        let json:object[] = res.map(x=>{
            let obj = {...x};
            obj[ExecutionReportField.Date] = moment(x[ExecutionReportField.Date]).format("DD-MMM-YY");                    
            if(Object.keys(res[0]).includes(ExecutionReportField.StartTime)){
                let startTime = x[ExecutionReportField.StartTime];
                console.log("start time",startTime)                ;
                if(!!startTime) obj[ExecutionReportField.StartTime] = moment(startTime).format("HH:mm:SS A");                 
            }
            if(Object.keys(res[0]).includes(ExecutionReportField.EndTime)){
                let endTime = x[ExecutionReportField.EndTime];
                console.log("end time",endTime);
                if(!!endTime) obj[ExecutionReportField.EndTime] = moment(endTime).format("HH:mm:SS A");                 
            }            

            if(Object.keys(res[0]).includes(ExecutionReportField.TotalDuration)){
                if(!!x[ExecutionReportField.EndTime] && !!x[ExecutionReportField.StartTime]){
                    let diff = moment.duration(moment(x[ExecutionReportField.EndTime]).diff(x[ExecutionReportField.StartTime]));
                    let hours = Utility.formateDigitCount(diff.hours(),2);
                    let minutes = Utility.formateDigitCount(diff.minutes(),2);
                    let seconds = Utility.formateDigitCount(diff.seconds(),2);                
                    obj[ExecutionReportField.TotalDuration] = `${hours}:${minutes}:${seconds}`;
                }
                
            }

            return obj;
        });
        this.AddSumObject(sumFields,json);        

        return json;
    }
}