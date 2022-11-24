import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity/Response/api-response';
import { AvCommunication } from '../../Entity/AVCommunications/avCommunication';
import { CommonService } from '../Common/common.service';
import { IPaginator } from '../../interfaces';
import { AvSetup } from '../../Entity/AVCommunications/avSetup';
import { CommunicationSetup } from '../../Entity/AVCommunications/communicationSetup';
@Injectable({
    providedIn: 'root'
})
export class AvCommunicationService {
    public baseUrl: string;
    public avSetupUrl:string;
    public communicationSetupUrl:string;
    

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private commonService:CommonService) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
        this.avSetupUrl = baseUrl + 'api/v1/AvSetup';
        this.communicationSetupUrl = baseUrl + 'api/v1/CommunicationSetup';
    }

    public save(model:AvCommunication) {
        return this.http.post<APIResponse>(this.baseUrl + 'v1/AvCommunication/save', 
        this.commonService.toFormData(model));
    }
    public update(model:AvCommunication) {
        return this.http.post<APIResponse>(this.baseUrl + 'v1/AvCommunication/Update', 
        this.commonService.toFormData(model));
    }
    public getAll() {
        return this.http.get<AvCommunication[]>(this.baseUrl + 'v1/AvCommunication/get');
    }
    public getById(id:number) {
        return this.http.get<AvCommunication>(`${this.baseUrl}v1/AvCommunication/get/${id}`);
    }

    public delete(id: number) {
        return this.http.delete<any>(`${this.baseUrl}v1/AvCommunication/delete/${id}`);
    }

    public getAvSetups(pageIndex:number, pageSize:number, search:string,fromDate:string,toDate:string,salesPointId:number) {
        return this.http.get<IPaginator<AvSetup>>(`${this.avSetupUrl}/getAvSetups?pageSize=${pageSize}&pageIndex=${pageIndex}&search=${search}&fromDateTime=${fromDate}&toDateTime=${toDate}&salespointId=${salesPointId}`);
    }
    public getExistingAvSetup(model: {data:AvSetup[]}) {
        return this.http.post<AvSetup[]>(`${this.avSetupUrl}/getExistingAvSetups`, model);
    }
    public createNewAvSetup(model: {data:AvSetup[]}) {
        return this.http.post(`${this.avSetupUrl}`, model);
    }
    public editAvSetup(model: AvSetup) {
        return this.http.put(`${this.avSetupUrl}`, model);
    }
    public getAvSetupById(id: number) {
        return this.http.get<AvSetup>(`${this.avSetupUrl}/getAvSetup/${id}`);
    }


    public getCommunicationSetups(pageIndex:number, pageSize:number, search:string,fromDate:string,toDate:string,salesPointId:number) {
        return this.http.get<IPaginator<CommunicationSetup>>(`${this.communicationSetupUrl}/Get?pageSize=${pageSize}&pageIndex=${pageIndex}&search=${search}&fromDateTime=${fromDate}&toDateTime=${toDate}&salespointId=${salesPointId}`);
    }
    public getExistingCommunicationSetup(model: {data:CommunicationSetup[]}) {
        return this.http.post<CommunicationSetup[]>(`${this.communicationSetupUrl}/getExistingCommunicationSetups`, model);
    }
    public createNewCommunicationsetup(model: {data:CommunicationSetup[]}) {
        return this.http.post(`${this.communicationSetupUrl}/save`, model);
    }
    public editCommunicationSetup(model: CommunicationSetup) {
        return this.http.put(`${this.communicationSetupUrl}/update`, model);
    }
    public getCommunicationSetupById(id: number) {
        return this.http.get<CommunicationSetup>(`${this.communicationSetupUrl}/get/${id}`);
    }
    
    
}
