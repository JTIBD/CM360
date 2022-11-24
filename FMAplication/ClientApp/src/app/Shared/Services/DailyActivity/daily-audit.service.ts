import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity/Response/api-response';
import { IPaginator } from '../../interfaces';
import { AuditSetup } from '../../Entity/Daily-Audit';

@Injectable({
  providedIn: 'root'
})
export class DailyAuditService {




  public baseUrl: string;
  public baseAuditUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
      console.log("baseUrl: ", baseUrl);
      this.baseUrl = baseUrl + 'api/';
      this.baseAuditUrl = baseUrl+'api/v1/dailyaudit'
  }

  getDailyAuditList() {
      return this.http.get<APIResponse>(this.baseUrl + 'v1/dailyaudit');
  }


   postDailyAudit(model) {
        //debugger;
      return this.http.post<APIResponse>(this.baseUrl + 'v1/dailyaudit/save', model);
  }

  getDropdownValue() {
      return this.http.get<APIResponse>(this.baseUrl + 'v1/dailyaudit/dropdown');
  }

    deleteDailyAudit(id: number) {
        return this.http.delete<any>(`${this.baseUrl}v1/dailyaudit/delete/${id}`);
    }

    getDailyAudit(id: number) {
        return this.http.get(`${this.baseUrl}v1/dailyaudit/${id}`);
    }

    getAuditSetups(pageIndex: number, pageSize: number, search: string, fromDate: string, toDate: string, salesPointId: number) {
        return this.http.get<IPaginator<AuditSetup>>(`${this.baseUrl}v1/dailyaudit/getAuditSetups?pageSize=${pageSize}&pageIndex=${pageIndex}&search=${search}&fromDateTime=${fromDate}&toDateTime=${toDate}&salesPointId=${salesPointId}`);
    }

    getExistingAuditSetups(payload: { data: AuditSetup[]; }) {
        return this.http.post<AuditSetup[]>(`${this.baseAuditUrl}/getExistingAuditSeups`, payload);
    }

    createNewAuditSetup(payload: { data: AuditSetup[]; }) {
        return this.http.post(`${this.baseAuditUrl}`, payload);
    }
    
    getAuditSetupById(auditSetupId: any) {
        return this.http.get<AuditSetup>(`${this.baseAuditUrl}/getAuditSetup/${auditSetupId}`);
    }

    editAuditSetup(auditSetup: AuditSetup) {
        return this.http.put<AuditSetup>(`${this.baseAuditUrl}`,auditSetup);
    }
 
}
