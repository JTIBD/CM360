import { HttpClient } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';
import { APIResponse } from '../../Entity';
import { TransactionNotification } from '../../Entity/Notification/TransactionNotification';

@Injectable({
  providedIn: 'root'
})
export class TransactionWorkflowService {

  public baseUrl: string;
    public transactionWorkflowURL:string;
    
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/v1/';
        this.transactionWorkflowURL = this.baseUrl + 'TransactionWorkflow';
    }
    acceptWorkFlow(model){
      return this.http.post<any>(this.transactionWorkflowURL + '/accept', model);
    }
    rejectWorkFlow(model){
      return this.http.post<any>(this.transactionWorkflowURL + '/reject', model);
    }
}
