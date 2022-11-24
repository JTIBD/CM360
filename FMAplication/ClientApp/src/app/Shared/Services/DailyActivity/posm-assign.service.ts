import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { CommonService } from '../Common/common.service';
import { PosmAssign, PosmAssignDetails, PosmParams } from '../../Entity/PosmAssign/posmAssign';
import { IPaginator } from '../../interfaces';


@Injectable({
  providedIn: 'root'
})
export class PosmAssignService {
  public baseUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private commonService: CommonService) { 
    this.baseUrl=baseUrl+"api/"    
  }

  getPosmAssigns(data : PosmParams) {
    let params = new HttpParams();

    params = params.append('pageSize', data.pageSize.toString());
    params = params.append('pageIndex', data.pageIndex.toString()); 
    params = params.append('fromDate', data.fromDate.toString()); 
    params = params.append('toDate', data.toDate.toString()); 
    if (data.search.length > 0) params = params.append('search', data.search); 
    if (data.salesPointId > 0) params = params.append('salesPointId', data.salesPointId.toString()); 

    return this.http.get<IPaginator<PosmAssign>>(`${this.baseUrl}v1/PosmAssign/GetPosmAssigns`,  {params});
  }

  getPosmAssignDetails(model) {
    let params = new HttpParams();

    params = params.append('cmUserId', model.cmUserId);
    params = params.append('salesPointId', model.salesPointId); 
    params = params.append('date', model.date);
    
    return this.http.get<PosmAssignDetails[]>(`${this.baseUrl}v1/PosmAssign/GetPosmAssignDetails`,  {params});
  }

  downloadPosmAssignExcelFile(downloadPosmFileModel) {
    return this.http.post(this.baseUrl + "v1/PosmAssign/DownloadPosmAssignFile", downloadPosmFileModel,  { observe: 'response', responseType: 'blob' });
  }

  excelImportPosmAssign(model) {
    return this.http.post(`${this.baseUrl}v1/PosmAssign/ExcelImportPosmAssign`, this.commonService.toFormData(model));
  }
}

