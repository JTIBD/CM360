import { MinimumExecutionLimit } from './../../Entity/minimum-execution-limit/minimum-execution-limit';
import { HttpClient } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';
import { APIResponse } from '../../Entity';

@Injectable({
  providedIn: 'root'
})
export class MinimumExecutionLimitService {

  public baseUrl: string;
    public minimumExecutionLimitUrl:string;
    
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/v1/';
        this.minimumExecutionLimitUrl = this.baseUrl + 'ExecutionLimit';
    }

    public getExistingMinimumExecutionLimit(model: {data:MinimumExecutionLimit[]}) {
        return this.http.post<MinimumExecutionLimit[]>(`${this.minimumExecutionLimitUrl}/GetExistingMinimumExecutionLimit`, model);
    }
    
    public createNewMinimumExecutionLimit(model: {data:MinimumExecutionLimit[]}) {
        return this.http.post(`${this.minimumExecutionLimitUrl}/Create`, model);
    }

    public getAll(model: any){
        return this.http.get<APIResponse>(`${this.minimumExecutionLimitUrl}/GetAll?pageSize=${model.pageSize}&pageIndex=${model.pageIndex}&searchText=${model.searchText}&salespointId=${model.salesPointId}`);
    }

    public getById(id:number){
        return this.http.get<APIResponse>(`${this.minimumExecutionLimitUrl}/GetById/${id}`);
    }

    public updateMinimumExecutionLimit(model){
        return this.http.put<APIResponse>(`${this.minimumExecutionLimitUrl}/Update`, model);
    }

    public delete(id:number){
        return this.http.delete<any>(`${this.minimumExecutionLimitUrl}/Delete/${id}`);
    }
}
