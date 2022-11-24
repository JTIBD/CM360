import { HttpClient } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';
import { APIResponse } from '../../Entity';
import { TransactionNotification, Notification } from '../../Entity/Notification/TransactionNotification';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  public baseUrl: string;
    public NotificationURL:string;
    
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/v1/';
        this.NotificationURL = this.baseUrl + 'Notification';
    }

    getTransactionNotifications() {
      return this.http.get<Notification>(this.NotificationURL + '/notifications');
    }
    getAllNotifications() {
      return this.http.get<TransactionNotification[]>(this.NotificationURL + '/AllNotifications');
    }
    markAsRead() {
      return this.http.get<Notification>(this.NotificationURL + '/MarkRead');
    }
   
}
