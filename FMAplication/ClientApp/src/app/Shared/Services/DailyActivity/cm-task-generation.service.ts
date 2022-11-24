import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { APIResponse } from '../../Entity/Response/api-response';
import { DailyCMActivity } from '../../Entity/Daily-Activity/daily-cm-activity';
import { BatchStatusChange } from '../../Entity/Daily-Activity/batch-status-change';
import { StatusTypes } from "../../../Shared/Enums/statusTypes";
import { MapObject } from '../../Enums/mapObject';
import { DCMAReportsInDetailsResponse, GetExecutionReport } from '../../Entity/Reports';


@Injectable({
  providedIn: 'root'
})
export class CmTaskGenerationService {
  public baseUrl: string;
  status: MapObject[] = StatusTypes.statusType;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) { 
    this.baseUrl=baseUrl+"api/"    
  }

  getFMUsers(){
    return this.http.get<APIResponse>(this.baseUrl + 'v1/DailyCMActivity/GetFMUsers');
  }
  getChannels(){
    return this.http.get<APIResponse>(this.baseUrl + 'v1/DailyCMActivity/GetAllChannel');
  }

  getOutletsByChannelId(channelId:number, salespointId:number){
    return this.http.get<APIResponse>(this.baseUrl + 'v1/DailyCMActivity/GetOutletByChannel/'+channelId+'/'+salespointId);
  }
  
  getRoutesBySalesPointId(salesPointId:number){
    return this.http.get<APIResponse>(this.baseUrl + 'v1/DailyCMActivity/GetRouteBySalesPoint/'+salesPointId);
  }

  getRoutesByFMId(id:number){
    return this.http.get<APIResponse>(this.baseUrl + 'v1/DailyCMActivity/GetRouteByFM/'+id);
  }

  getSalesPointByFMId(id:number){
    return this.http.get<APIResponse>(this.baseUrl + 'v1/DailyCMActivity/GetSalesPointByFM/'+id);
  }

  getOutletsByRouteId( routeId:number){
    return this.http.get<APIResponse>(this.baseUrl + 'v1/DailyCMActivity/GetOutletByRoute/'+routeId);
  }

  getCMUsersByFMId( fmId:number){
    return this.http.get<APIResponse>(this.baseUrl + 'v1/DailyCMActivity/GetCMUserByFM/'+fmId);
  }

  getPOSMProducts(){
    return this.http.get<APIResponse>(this.baseUrl + 'v1/posmproduct/approved');
  }
  getProducts(){
    return this.http.get<APIResponse>(this.baseUrl + 'v1/product');
  }  

  getCMTask(){
    return this.http.get<APIResponse>(this.baseUrl + 'v1/DailyCMActivity/GetCMTask');
  }

    getDailyCMActivityList(pageIndex, pageSize, search) {
        var params = new HttpParams();
        params=params.append("pageIndex", pageIndex);
        params=params.append("pageSize", pageSize);
        params=params.append("search", search);
     

        return this.http.get<APIResponse>(this.baseUrl + 'v1/DailyCMActivity/GetDailyCMActivitiesByCurrentUser', { params: params });
    }

  getAllReportData(){
    return this.http.get<APIResponse>(this.baseUrl + 'v1/DailyCMActivity/GetCompleteReport');
  }

  public updateStatus(model) {

    return this.http.post<APIResponse>(this.baseUrl + 'v1/DailyCMActivity/updateStatus', model);
  }

  public updateBatchStatus(model: BatchStatusChange) {

    return this.http.post<APIResponse>(this.baseUrl + 'v1/DailyCMActivity/updateBatchStatus', model);
  }

    getSurveyReports(pageIndex, pageSize, search) {
        var params = new HttpParams()
        params = params.append("pageIndex", pageIndex);
        params = params.append("pageSize", pageSize);
        params = params.append("search", search);
        return this.http.get<APIResponse>(this.baseUrl + 'v1/DailyCMActivity/GetSurveyReport', { params: params });
  }

    getPOSMReports(pageIndex, pageSize, search) {
        var params = new HttpParams()
        params = params.append("pageIndex", pageIndex);
        params = params.append("pageSize", pageSize );
        params = params.append("search", search);
        return this.http.get<APIResponse>(this.baseUrl + 'v1/DailyCMActivity/GetPOSMReport', { params: params });
    }

    getAuditReports(pageIndex, pageSize, search) {
        var params = new HttpParams()
        params = params.append("pageIndex", pageIndex);
        params = params.append("pageSize", pageSize);
        params = params.append("search", search);
        return this.http.get<APIResponse>(this.baseUrl + 'v1/DailyCMActivity/GetAuditReport', { params: params });
  }

    getDCMAReportsInDetails(pageIndex, pageSize, search) {
        var params = new HttpParams()
        params = params.append("pageIndex", pageIndex);
        params = params.append("pageSize", pageSize);
        params = params.append("search", search);
        // return this.http.get<APIResponse>(this.baseUrl + 'v1/DailyCMActivity/GetDCMAReportsInDetails', { params: params });
        return this.http.get<DCMAReportsInDetailsResponse>(this.baseUrl + 'v1/DailyCMActivity/GetDCMAReportsInDetails2', { params: params });
  }

    getDCMAReportsSalesPointWise(pageIndex, pageSize, search) {
        var params = new HttpParams()
        params = params.append("pageIndex", pageIndex);
        params = params.append("pageSize", pageSize);
        params = params.append("search", search);
//        return this.http.get<APIResponse>(this.baseUrl + 'v1/DailyCMActivity/GetDCMAReportsSalesPointWise', { params: params });
        return this.http.get<DCMAReportsInDetailsResponse>(this.baseUrl + 'v1/DailyCMActivity/GetDCMAReportsSalesPointWise2', { params: params });
  }

    getDCMAReportsTerritoryWise(pageIndex, pageSize, search) {
        var params = new HttpParams()
        params = params.append("pageIndex", pageIndex);
        params = params.append("pageSize", pageSize);
        params = params.append("search", search);
        // return this.http.get<APIResponse>(this.baseUrl + 'v1/DailyCMActivity/GetDCMAReportsTerritoryWise', { params: params });
        return this.http.get<DCMAReportsInDetailsResponse>(this.baseUrl + 'v1/DailyCMActivity/GetDCMAReportsTerritoryWise2', { params: params });
  }

    getDCMAReportsAreaWise(pageIndex, pageSize, search) {
        var params = new HttpParams()
        params = params.append("pageIndex", pageIndex);
        params = params.append("pageSize", pageSize);
        params = params.append("search", search);
        // params = params.append("isAll", isAll);
        // return this.http.get<APIResponse>(this.baseUrl + 'v1/DailyCMActivity/GetDCMAReportsAreaWise', { params: params });
        return this.http.get<DCMAReportsInDetailsResponse>(this.baseUrl + 'v1/DailyCMActivity/GetDCMAReportsAreaWise2', { params: params });
  }

    getDCMAReportsRegionWise(pageIndex, pageSize, search) {
        var params = new HttpParams()
        params = params.append("pageIndex", pageIndex);
        params = params.append("pageSize", pageSize);
        params = params.append("search", search);        
        // return this.http.get<APIResponse>(this.baseUrl + 'v1/DailyCMActivity/GetDCMAReportsRegionWise', { params: params });
        return this.http.get<DCMAReportsInDetailsResponse>(this.baseUrl + 'v1/DailyCMActivity/GetDCMAReportsRegionWise2', { params: params });
  }

  saveCMTask(model: DailyCMActivity[]) {
    model.forEach(element => {
      element.id=0;
      const mapObject = this.status.filter(k => k.id == 2)[0];
      element.status = mapObject.id;
      if(!element.isPOSM) {
        element.dailyPOSM = null;
      } else {
        if(!element.dailyPOSM.isPOSMInstallation) {
          element.dailyPOSM.posmInstallationProducts = [];
        }
        if(!element.dailyPOSM.isPOSMRepair) {
          element.dailyPOSM.posmRepairProducts = [];
        }
        if(!element.dailyPOSM.isPOSMRemoval) {
          element.dailyPOSM.posmRemovalProducts = [];
        }
      }
      
      if(!element.isAudit) {
        element.dailyAudit = null;
      } else {
        if(!element.dailyAudit.isDistributionCheck) {
          element.dailyAudit.distributionCheckProducts = [];
        }
        if(!element.dailyAudit.isFacingCount) {
          element.dailyAudit.facingCountProducts = [];
        }
        if(!element.dailyAudit.isPlanogramCheck) {
          element.dailyAudit.planogramCheckProducts = [];
        }
        if(!element.dailyAudit.isPriceAudit) {
          element.dailyAudit.priceAuditProducts = [];
        }
      }
    });
    console.log('cm task post data: ',model);
    return this.http.post<APIResponse>(this.baseUrl + 'v1/DailyCMActivity/SaveCMTask', model);
  }

  getExecutionReportSalesPointWise(payload:GetExecutionReport) {
    return this.http.post<object[]>(this.baseUrl + 'v1/DailyCMActivity/GetExecutionReportSalesPointWise', payload);
  }

  getExecutionReportOutletWise(payload:GetExecutionReport) {
    return this.http.post<object[]>(this.baseUrl + 'v1/DailyCMActivity/GetExecutionReportOutletWise', payload);
  }

  getExecutionReportTeritoryWise(payload:GetExecutionReport) {
    return this.http.post<object[]>(this.baseUrl + 'v1/DailyCMActivity/GetExecutionReportTeritoryWise', payload);
  }

  getExecutionReportAreaWise(payload:GetExecutionReport) {
    return this.http.post<object[]>(this.baseUrl + 'v1/DailyCMActivity/GetExecutionReportAreaWise', payload);
  }

  getExecutionReportRegionWise(payload:GetExecutionReport) {
    return this.http.post<object[]>(this.baseUrl + 'v1/DailyCMActivity/GetExecutionReportRegionWise', payload);
  }

  getExecutionReportNational(payload:GetExecutionReport) {
    return this.http.post<object[]>(this.baseUrl + 'v1/DailyCMActivity/GetExecutionReportNational', payload);
  }
}
