import { GuidelineSetup } from 'src/app/Shared/Entity/Guidelines/guideline-setup';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../Common/common.service';
import { IGetGuidelineSetupQueryModel } from '../../Entity/Guidelines/guideline-setup';
import { Inject, Injectable } from '@angular/core';
import { IPaginator } from '../../interfaces';
import { APIResponse } from '../../Entity/Response/api-response';

@Injectable({
    providedIn: 'root'
})

export class GuidelineSetupService{
    public baseUrl: string;
    public guidelineSetupUrl:string;
    

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/v1/';
        this.guidelineSetupUrl = this.baseUrl + 'GuidelineSetup';
    }

    public getExistingGuidelineSetup(model: {data:GuidelineSetup[]}) {
        return this.http.post<GuidelineSetup[]>(`${this.guidelineSetupUrl}/GetExistingGuidelineSetups`, model);
    }
    
    public createNewGuidelineSetup(model: {data:GuidelineSetup[]}) {
        return this.http.post(`${this.guidelineSetupUrl}/Create`, model);
    }

    public getAll(model: IGetGuidelineSetupQueryModel){
        return this.http.get<APIResponse>(`${this.guidelineSetupUrl}/GetAll?pageSize=${model.pageSize}&pageIndex=${model.pageIndex}&search=${model.search}&fromDateTime=${model.fromDateTime}&toDateTime=${model.toDateTime}&salespointId=${model.salesPointId}`);
    }

    public getById(id:number){
        return this.http.get<APIResponse>(`${this.guidelineSetupUrl}/GetById/${id}`);
    }

    public updateGuidelineSetup(model){
        return this.http.put<APIResponse>(`${this.guidelineSetupUrl}/Update`, model);
    }
}