import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity/Response/api-response';
@Injectable({
    providedIn: 'root'
})
export class SubBrandService {
    public baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    public getSubBrandList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/subBrand');
    }

    public getAllForSelect(brandId: number) {
        return this.http.get<APIResponse>(this.baseUrl + `v1/subBrand/select/${brandId}`);
    }

    public getSubBrand(id: number) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/subBrand/'+id);
    }

    public postSubBrand(model) {
        return this.http.post<APIResponse>(this.baseUrl + 'v1/subBrand/create', model);
    }

    public putSubBrand(model) {
        return this.http.put<APIResponse>(this.baseUrl + 'v1/subBrand/update', model);
    }

    public deleteSubBrand(id: number) {
        return this.http.delete<any>(`${this.baseUrl}v1/subBrand/delete/${id}`);
    }
}
