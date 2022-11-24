import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIModel, APIResponse } from '../../Entity/Response/api-response';
import { CommonService } from '../Common/common.service';
import { PosmProduct } from '../../Entity';
@Injectable({
    providedIn: 'root'
})
export class PosmProductService {
    public baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string,
        private commonService: CommonService) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    getPosmProductList() {
        return this.http.get<APIResponse<APIModel<PosmProduct[]>>>(this.baseUrl + 'v1/posmproduct');
    }

    public getPosmProduct(id: number) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/posmproduct/' + id);
    }

    public getAllJtiPosmProducts() {
        return this.http.get<PosmProduct[]>(this.baseUrl + 'v1/posmproduct/allJTIProducts');
    }


    public postPosmProduct(model) {
        return this.http.post<APIResponse>(this.baseUrl + 'v1/posmproduct/save', 
            this.commonService.toFormData(model));
    }

    public putPosmProduct(model) {
        return this.http.put<APIResponse>(this.baseUrl + 'v1/posmproduct/update', 
            this.commonService.toFormData(model));
    }

    public deletePosmProduct(id: number) {
        return this.http.delete<APIResponse>(`${this.baseUrl}v1/posmproduct/delete/${id}`);
    }
}

