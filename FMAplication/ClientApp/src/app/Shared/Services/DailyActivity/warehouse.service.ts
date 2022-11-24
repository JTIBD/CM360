import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../Common/common.service';
import { IPaginator } from '../../interfaces';
import { DownloadExcelForWarehouseTransfer, WareHouseReceivedTransfer, WareHouseStock, WareHouseTransfer } from '../../Entity/wareHouse';
import { WareHouse } from '../../Entity/Inventory';


@Injectable({
  providedIn: 'root'
})
export class WareHouseService {
  
  public baseUrl: string;
  public wareHouseUrl:string;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private commonService: CommonService) { 
    this.baseUrl=baseUrl+"api/"    
    this.wareHouseUrl=baseUrl+"api/v1/WareHouse"    
  }

  getStocks(pageIndex:number,pageSize:number,search:string,wareHouseIds:number[]){    
    return this.http.post<IPaginator<WareHouseStock>>(`${this.wareHouseUrl}/getStocks`,{
      pageIndex,pageSize,search,wareHouseIds
    });
  }
  getWareHousesByCodes(codes:string[]){
    return this.http.post<WareHouse[]>(this.wareHouseUrl + '/GetByCodes',{data:codes});
  }
  getTransfers(pageIndex:number, pageSize:number, search:string,fromDate:string,toDate:string){
    return this.http.get<IPaginator<WareHouseTransfer>>(this.wareHouseUrl + '/GetWareHouseTransfers?pageSize='+ pageSize+"&pageIndex="+pageIndex+"&search="+search+"&fromDateTime="+fromDate+"&toDateTime="+toDate);
  }
  downloadWareHouseTransferExcel(payload: DownloadExcelForWarehouseTransfer) {
    return this.http.post(this.wareHouseUrl + '/DownloadExcelForWareHouseDistribution',payload, { responseType: 'blob' });
  }
  createTransfer(payload: WareHouseTransfer) {
    return this.http.post<WareHouseTransfer>(this.wareHouseUrl + '/AddWareHouseTransfer', payload);
  }

  confirmTransfer(transactionId:number){
    return this.http.put<WareHouseTransfer>(this.wareHouseUrl + '/ConfirmWareHouseTransfer?transactionId='+transactionId,{});
  }

  getReceivablePOSMDistributionByCurrentUser(){
    return this.http.get<WareHouseTransfer[]>(this.wareHouseUrl + "/GetReceivableTransfers");
  }
  
  recieveTransfer(transaction:WareHouseReceivedTransfer){
    return this.http.post<WareHouseReceivedTransfer>(this.wareHouseUrl + '/RecieveTransfer', transaction);
  }

  getReceivedTransfers(pageIndex:number, pageSize:number, search:string,fromDate:string,toDate:string){
    return this.http.get<IPaginator<WareHouseReceivedTransfer>>(this.wareHouseUrl + '/GetReceivedTransfers?pageSize='+ pageSize+"&pageIndex="+pageIndex+"&search="+search+"&fromDateTime="+fromDate+"&toDateTime="+toDate);
  }

  
}

