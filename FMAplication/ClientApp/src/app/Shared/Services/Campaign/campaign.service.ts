import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity/Response/api-response';
@Injectable({
    providedIn: 'root'
})
export class CampaignService {
    public baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    public getCampaignList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/campaign');
    }

    public getAllForSelect() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/campaign/select');
    }

    public getCampaign(id: number) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/campaign/'+id);
    }

    public postCampaign(model) {
        return this.http.post<APIResponse>(this.baseUrl + 'v1/campaign/create', model);
    }

    public putCampaign(model) {
        return this.http.put<APIResponse>(this.baseUrl + 'v1/campaign/update', model);
    }

    public deleteCampaign(id: number) {
        return this.http.delete<any>(`${this.baseUrl}v1/campaign/delete/${id}`);
    }
}
