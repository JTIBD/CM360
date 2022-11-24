import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity/Response/api-response';
@Injectable({
    providedIn: 'root'
})
export class BrandService {
    public baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    public getBrandList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/brand');
    }

    public getAllForSelect() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/brand/select');
    }

    public getBrand(id: number) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/brand/'+id);
    }

    public postBrand(model) {
        return this.http.post<APIResponse>(this.baseUrl + 'v1/brand/create', model);
    }

    public putBrand(model) {
        return this.http.put<APIResponse>(this.baseUrl + 'v1/brand/update', model);
    }

    public deleteBrand(id: number) {
        return this.http.delete<any>(`${this.baseUrl}v1/brand/delete/${id}`);
    }
}
