import { HttpClient, HttpParams  } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { CreatePOSM_Distribution } from 'src/app/Shared/Entity';
import { AdjustmentTransactionParams, IStockAdjustmentTransaction } from 'src/app/Shared/Entity/Inventory/StockAdjustmentTransaction';
import {  IPosmProductStock } from 'src/app/Shared/Entity/Inventory/StockProduct';
import { TransactionStatus } from 'src/app/Shared/Enums/TransactionStatus';
import { IPaginator } from 'src/app/Shared/interfaces';
import { CreateStockAddTransaction, DownloadExcelForStockDistributions, UpdateStockAddTransaction, WareHouse } from '../../Shared/Entity/Inventory';
import { Transaction } from '../../Shared/Entity/Inventory/Transaction';
import { MapObject } from '../../Shared/Enums/mapObject';
import { StatusTypes } from '../../Shared/Enums/statusTypes';
import { TransactionType } from '../../Shared/Enums/TransactionType';

@Injectable({
  providedIn: 'root'
})
export class InventoryManagementService {

    public baseUrl: string;
    status: MapObject[] = StatusTypes.statusType;
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.baseUrl = baseUrl + "api/"
    }

    downloadStockCreationExcel(wareHouseId: number) {
        return this.http.get(this.baseUrl + 'v1/Inventory/DownloadExcelForStockAdd/' + wareHouseId, { responseType: 'blob' });
    }

    downloadStockDistributionExcel(payload: DownloadExcelForStockDistributions) {
        return this.http.post(this.baseUrl + 'v1/Inventory/DownloadExcelForWStockDistribution',payload, { responseType: 'blob' });
    }

    downloadStockAdjustExcel() {        
        return this.http.get(this.baseUrl + 'v1/Inventory/DownloadExcelForStockAdjust', { responseType: 'blob' });
    }

    getWarehouseStockData(wareHouseId:number) {
        return this.http.get<IPosmProductStock[]>(this.baseUrl + 'v1/Inventory/GetProductStocks/' + wareHouseId);
    }
    getStockAdjustmentTransaction() {
        return this.http.get<IStockAdjustmentTransaction>(this.baseUrl + 'v1/Inventory/GetAdjustmentTransaction');
    }
    saveStockAdjustmentTransaction(transaction:IStockAdjustmentTransaction) {
        return this.http.post<IStockAdjustmentTransaction>(this.baseUrl + 'v1/Inventory/SaveAdjustmentTransaction', transaction);
    }
    saveSalesPointStockAdjustmentTransaction(transaction:Transaction) {
        return this.http.post<IStockAdjustmentTransaction>(this.baseUrl + 'v1/Inventory/SaveSalesPointAdjustmentTransaction', transaction);
    }
    UpdateStockAdjustmentTransaction(transaction:IStockAdjustmentTransaction) {
        return this.http.post<IStockAdjustmentTransaction>(this.baseUrl + 'v1/Inventory/UpdateAdjustmentTransaction', transaction);
    }
    UpdateSalesPointStockAdjustmentTransaction(transaction:Transaction) {
        return this.http.post<Transaction>(this.baseUrl + 'v1/Inventory/UpdateSalesPointAdjustmentTransaction', transaction);
    }
    getStockAdjustmentTransactionById(transeactionId:number) {
        return this.http.get<IStockAdjustmentTransaction>(this.baseUrl + 'v1/Inventory/GetAdjustmentTransactionById/'+ transeactionId);
    }
    getSalesPointStockAdjustmentTransactionById(transeactionId:number) {
        return this.http.get<Transaction>(this.baseUrl + 'v1/Inventory/GetSalesPointAdjustmentTransactionById/'+ transeactionId);
    }

    getStockDistriTransactions(pageIndex:number, pageSize:number, search:string,fromDate:string,toDate:string){
        return this.http.get<IPaginator<Transaction>>(this.baseUrl + 'v1/Inventory/GetStockDistributionTransactions?pageSize='+ pageSize+"&pageIndex="+pageIndex+"&search="+search+"&fromDateTime="+fromDate+"&toDateTime="+toDate);
    }
    getStockReceiveTransactions(pageIndex:number, pageSize:number, search:string,fromDate:string,toDate:string){
        return this.http.get<IPaginator<Transaction>>(this.baseUrl + 'v1/Inventory/GetReceivedTransactions?pageSize='+ pageSize+"&pageIndex="+pageIndex+"&search="+search+"&fromDateTime="+fromDate+"&toDateTime="+toDate);
    }

    getStockDistributionTotalTransactionsCount(){
        return this.http.get<number>(this.baseUrl + 'v1/Inventory/GetStockDistributionTotalTransactionsCount');
    }

    getWareHouses() {
        return this.http.get<WareHouse[]>(this.baseUrl + 'v1/Inventory/GetWareHouses');
    }

    createAddStockTransaction(payload: CreateStockAddTransaction) {
        return this.http.post<Transaction>(this.baseUrl + 'v1/Inventory/AddStockTransaction', payload);
    }

    addWPOSM_DistributionTransaction(payload:CreatePOSM_Distribution){
        return this.http.post<Transaction[]>(this.baseUrl + 'v1/Inventory/AddWPOSM_DistributionTransaction', payload);
    }

    getTransactions(type: TransactionType) {
        return this.http.get<Transaction[]>(this.baseUrl + "v1/Inventory/GetTransactions?transactionType=" + type);
    }
    
    getStockAdjustmentTransactions(data:AdjustmentTransactionParams) {
        let params = new HttpParams();

        params = params.append('pageSize', data.pageSize.toString());
        params = params.append('pageIndex', data.pageIndex.toString()); 
        params = params.append('fromDate', data.fromDate.toString()); 
        params = params.append('toDate', data.toDate.toString()); 
        if (data.search.length > 0) params = params.append('search', data.search); 
        if (data.transactionStatus >= 0) params = params.append('transactionStatus', data.transactionStatus.toString()); 
        
       
        
        return this.http.get<IPaginator<IStockAdjustmentTransaction>>(this.baseUrl + "v1/Inventory/GetStockAdjustTransactions", {params} );
    }

    getSalesPointStockAdjustmentTransactions(pageIndex:number, pageSize:number, search:string,fromDate:string,toDate:string,transactionStatus:TransactionStatus){
        return this.http.get<IPaginator<Transaction>>(this.baseUrl + 'v1/Inventory/GetSalesPointStockAdjustTransactions?pageSize='+ pageSize+"&pageIndex="+pageIndex+"&search="+search+"&fromDateTime="+fromDate+"&toDateTime="+toDate+"&transactionStatus="+transactionStatus);
    }
    getStockAddTransactions(pageIndex:number, pageSize:number, search:string,fromDate:string,toDate:string) {
        return this.http.get<IPaginator<Transaction>>(this.baseUrl + 'v1/Inventory/GetStockAddTransactions2?pageSize='+ pageSize+"&pageIndex="+pageIndex+"&search="+search+"&fromDateTime="+fromDate+"&toDateTime="+toDate);
    }
    updateTransaction(payload:Transaction){
        return this.http.put<Transaction>(this.baseUrl + 'v1/Inventory/UpdateTransaction', payload);
    }
    confirmStockAddTransaction(transactionId:number){
        return this.http.put<Transaction>(this.baseUrl + 'v1/Inventory/ConfirmStockAddTransaction?transactionId='+transactionId,{});
    }
    confirmStockDistributionTransaction(transactionId:number){
        return this.http.put<Transaction>(this.baseUrl + 'v1/Inventory/ConfirmWDistributionTransaction?transactionId='+transactionId,{});
    }
    
    updateStockAddTransaction(payload:UpdateStockAddTransaction){
        return this.http.put<Transaction>(this.baseUrl + 'v1/Inventory/UpdateStockAddTransaction',payload);
    }
    confirmStockAdjustmentTransaction(transactionId:number){
        return this.http.put<Transaction>(this.baseUrl + 'v1/Inventory/ConfirmStockAdjustTransaction?transactionId='+transactionId,{});
    }
    confirmSalesPointStockAdjustmentTransaction(transactionId:number){
        return this.http.put<Transaction>(this.baseUrl + 'v1/Inventory/ConfirmSalesPointStockAdjustTransaction?transactionId='+transactionId,{});
    }
    getReceivablePOSMDistributionByCurrentUser(){
        return this.http.get<Transaction[]>(this.baseUrl + "v1/Inventory/GetReceivablePOSMDistributionByCurrentUser");
    }
    recievePOSM_Stock(transaction:Transaction){
        return this.http.post<Transaction>(this.baseUrl + 'v1/Inventory/RecievePOSM_Stock', transaction);
    }
}
