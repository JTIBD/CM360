import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { DailyTask } from '../../Entity/DailyTasks';
import { IPaginationParams, IPaginator } from '../../interfaces';
import { SalesPointMultiSelectItem } from '../../Entity/Reports/ISalespointMultiSelectSettings';



@Injectable({
    providedIn: 'root'
  })
export class ReportService {
    
    public baseUrl: string;
    public reportBaseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {        
        this.baseUrl = baseUrl + 'api/';
        this.reportBaseUrl = baseUrl + 'api/v1/Report';
    }

    getAuditReports(queryParams:IPaginationParams) {
        let params = new HttpParams({fromObject:queryParams as any});
        return this.http.get<IPaginator<DailyTask>>(`${this.reportBaseUrl}/GetAuditReport`,{params});
    } 
    getSurveyReports(queryParams:IPaginationParams) {
        let params = new HttpParams({fromObject:queryParams as any});
        return this.http.get<IPaginator<DailyTask>>(`${this.reportBaseUrl}/GetSurveyReport`,{params});
    } 
    getConsumerSurveyReports(queryParams:IPaginationParams) {
        let params = new HttpParams({fromObject:queryParams as any});
        return this.http.get<IPaginator<DailyTask>>(`${this.reportBaseUrl}/GetConsumerSurveyReport`,{params});
    } 
    getAVReports(queryParams:IPaginationParams) {
        let params = new HttpParams({fromObject:queryParams as any});
        return this.http.get<IPaginator<DailyTask>>(`${this.reportBaseUrl}/GetAvReport`,{params});
    } 
    getCommunicationReport(queryParams:IPaginationParams) {
        let params = new HttpParams({fromObject:queryParams} as any);
        return this.http.get<IPaginator<DailyTask>>(`${this.reportBaseUrl}/GetCommunicationReport`,{params});
    } 

    getInformationReport(queryParams:IPaginationParams) {
        let params = new HttpParams({fromObject:queryParams as any});
        return this.http.get<IPaginator<DailyTask>>(`${this.reportBaseUrl}/GetInformationReport`,{params});
    }
    getPOSMReport(queryParams:IPaginationParams) {
        let params = new HttpParams({fromObject:queryParams as any});
        return this.http.get<IPaginator<DailyTask>>(`${this.reportBaseUrl}/GetPOSMTaskReport`,{params});
    } 
    exportAuditReportToExcel(queryParams:IPaginationParams) {
        let params = new HttpParams({fromObject:queryParams as any});
        return this.http.get(`${this.reportBaseUrl}/ExportAuditReportToExcel`,{ params,responseType: 'blob'});
    }
    exportSurveyReportToExcel(queryParams:IPaginationParams) {
        let params = new HttpParams({fromObject:queryParams as any});
        return this.http.get(`${this.reportBaseUrl}/ExportSurveyReportToExcel`,{ params,responseType: 'blob'});
    }
    exportConsumerSurveyReportToExcel(queryParams:IPaginationParams) {
        let params = new HttpParams({fromObject:queryParams as any});
        return this.http.get(`${this.reportBaseUrl}/ExportConsumerSurveyReportToExcel`,{ params,responseType: 'blob'});
    }
    exportAvReportToExcel(queryParams:IPaginationParams) {
        let params = new HttpParams({fromObject:queryParams as any});
        return this.http.get(`${this.reportBaseUrl}/ExportAvReportToExcel`,{ params,responseType: 'blob'});
    }
    exportCommunicationReportToExcel(queryParams:IPaginationParams) {
        let params = new HttpParams({fromObject:queryParams as any});
        return this.http.get(`${this.reportBaseUrl}/ExportCommunicationReportToExcel`,{ params,responseType: 'blob'});
    }
    exportInformationReportToExcel(queryParams:IPaginationParams) {
        let params = new HttpParams({fromObject:queryParams as any});
        return this.http.get(`${this.reportBaseUrl}/ExportInformationReportToExcel`,{ params,responseType: 'blob'});
    }
    exportPOSMTaskReportToExcel(queryParams:IPaginationParams) {
        let params = new HttpParams({fromObject:queryParams as any});
        return this.http.get(`${this.reportBaseUrl}/ExportPOSMTaskReportToExcel`,{ params,responseType: 'blob'});
    }

    exportCWStockUpdateReportToExcel(fromDateTime:string, toDateTime:string, cwIds: number[]) {
        let params = new HttpParams({fromObject:{fromDateTime, toDateTime, cwIds} as any});
        return this.http.get(`${this.reportBaseUrl}/ExportCWStockUpdateReportToExcel`, { params, responseType: 'blob' })
    }
    
    exportCWDistributionReportToExcel(fromDateTime:string,toDateTime:string,salesPointIds:number[]) {
        return this.http.post(`${this.reportBaseUrl}/ExportCWDistributionReportToExcel`,{fromDateTime,toDateTime,salesPointIds},{ responseType: 'blob'});
    }

    exportCWStockReportToExcel(warHouseIds:number[]) {
        return this.http.post(`${this.reportBaseUrl}/ExportCWStockReportToExcel`,{data:warHouseIds},{ responseType: 'blob'});
    }

    exportSPStockReportToExcel(salesPointIds:number[]) {
        return this.http.post(`${this.reportBaseUrl}/ExportSPStockReportToExcel`,{data:salesPointIds},{ responseType: 'blob'});
    }

    exportSPWisePosmLedgerReport(fromDateTime:string,toDateTime:string,salesPointIds:number[],posmProductIds:number[]){
        return this.http.post(`${this.reportBaseUrl}/ExportSPWisePosmLedgerReportToExcel`,{fromDateTime,toDateTime,salesPointIds,posmProductIds},{ responseType: 'arraybuffer'});
    }
}