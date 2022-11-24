import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { APIResponse, SalesPoint, SalesPointTransfer, SalesPointReceivedTransfer } from '../../Entity';
import { DownloadExcelForSalesPointTransfer } from '../../Entity/Inventory';
import { SalespointStock } from '../../Entity/Sales/salespointStock';
import { TransactionStatus } from '../../Enums/TransactionStatus';
import { IPaginator } from '../../interfaces';



@Injectable({
    providedIn: 'root'
  })
export class SalesPointService {
    
    public baseUrl: string;
    public salesPointUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {        
        this.baseUrl = baseUrl + 'api/';
        this.salesPointUrl = baseUrl + 'api/v1/SalesPoint';
    }


    getAllSalesPoint() {
        return this.http.get<APIResponse<SalesPoint[]>>(this.baseUrl + 'v1/salespoint');
    }
    getAllSalesPointByCurrentFmUser(){
        return this.http.get<APIResponse>( `${this.salesPointUrl}/GetAllSalesPointByCurrentFmUser`);
    }
    getStocks(pageIndex: number, pageSize: number, search: string, salesPointIds: number[]) {
        return this.http.post<IPaginator<SalespointStock>>(`${this.salesPointUrl}/getStocks`,{pageIndex,pageSize,search,salesPointIds});
    }
    downloadSPTransferExcel(payload: DownloadExcelForSalesPointTransfer) {
        return this.http.post(`${this.salesPointUrl}/DownloadExcelForSalesPointTransfer`,payload, { responseType: 'blob' });
    }

    createTransfers(transfers: SalesPointTransfer[]) {
        return this.http.post(`${this.salesPointUrl}/AddSalesPointTransfer`,{data:transfers});
    }

    getTransfers(pageIndex:number, pageSize:number, search:string,fromDate:string,toDate:string,transactionStatus:TransactionStatus|-1){
        return this.http.get<IPaginator<SalesPointTransfer>>(this.salesPointUrl + '/GetSalesPointTransfers?pageSize='+ pageSize+"&pageIndex="+pageIndex+"&search="+search+"&fromDateTime="+fromDate+"&toDateTime="+toDate+"&transactionStatus="+transactionStatus);
    }

    confirmTransfer(id:number){
        return this.http.put<SalesPointTransfer>(this.salesPointUrl + '/ConfirmSalesPointTransfer?transactionId='+id,{});
    }

    getReceivedTransfers(pageIndex:number, pageSize:number, search:string,fromDate:string,toDate:string){
        return this.http.get<IPaginator<SalesPointReceivedTransfer>>(this.salesPointUrl + '/GetReceivedTransfers?pageSize='+ pageSize+"&pageIndex="+pageIndex+"&search="+search+"&fromDateTime="+fromDate+"&toDateTime="+toDate);
    }

    getReceivableSPTransfersByCurrentUser(){
        return this.http.get<SalesPointTransfer[]>(this.salesPointUrl + "/GetReceivableTransfers");
    }

    recieveTransfer(transaction:SalesPointReceivedTransfer){
        return this.http.post<SalesPointReceivedTransfer>(this.salesPointUrl + '/RecieveTransfer', transaction);
    }
    
    getSalesPointTransferById(id:number){
        return this.http.get<SalesPointTransfer>( `${this.salesPointUrl}/GetSalesPointTransferById/${id}`);
    }
}