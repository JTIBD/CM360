import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { DailyTask } from '../../Entity/DailyTasks';
import { IPaginator } from '../../interfaces';

@Injectable({
  providedIn: 'root'
})
export class DailyTaskService {
    public baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.baseUrl = baseUrl + 'api/v1/dailytask';
    }  
}
