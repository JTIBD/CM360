import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { IParsedExcel, IParseExcelModel } from '../../interfaces';
import { CommonService } from '../Common/common.service';

@Injectable({
  providedIn: 'root'
})
export class FileUtilityService {
    public baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string,
        private commonService: CommonService) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/v1';
    }
    ParseExcel(model: IParseExcelModel) {
        const url = this.baseUrl + "/FileUtility/ParseExcel";
        const formData = this.commonService.toFormData(model);
        return this.http.post<IParsedExcel>(url, formData);
    }
}
