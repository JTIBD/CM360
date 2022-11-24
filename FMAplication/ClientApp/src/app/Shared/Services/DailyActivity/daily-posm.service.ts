import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { APIResponse } from '../../Entity';

@Injectable({
  providedIn: 'root'
})
export class DailyPosmService {
    public baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    getDailyPOSMtList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/dailyposm');
    }


    public postDailyPOSM(model) {
        return this.http.post<APIResponse>(this.baseUrl + 'v1/dailyposm/save', model);
    }

    getDropdownValue() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/dailyposm/dropdown');
    }

    deleteDailyPOSM(id: number) {
        return this.http.delete<any>(`${this.baseUrl}v1/dailyposm/delete/${id}`);
    }

    getDailyPOSM(id: number) {
        return this.http.get(`${this.baseUrl}v1/dailyposm/${id}`);
    }
  
}
