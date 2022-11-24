import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { APIResponse } from '../../Entity';
import { Reason } from '../../Entity/ExecutionReason/ExecutionReason';
import { ReasonType } from '../../Entity/ExecutionReason/ReasonType';

@Injectable({
	providedIn: 'root'
})
export class ExecutionReasonService {
	public baseUrl: string;

	constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
		this.baseUrl = baseUrl + 'api/';
		console.log('base url:' + baseUrl);
	 }

	public getAllExecutionReasons() {
		return this.http.get<APIResponse>(this.baseUrl + 'v1/Reason/GetAllReasons');
	}

	public getReason(id: Number) {
		return this.http.get<APIResponse<Reason>>(this.baseUrl + 'v1/Reason/' + id);
	}

	public getReasonTypes() {
		return this.http.get<ReasonType[]>(this.baseUrl + 'v1/Reason/GetAllReasonTypes');
	}

	public updateExecutionReason(model) {
		return this.http.put<APIResponse>(this.baseUrl + 'v1/Reason/UpdateReason', model);
	}

	public createExecutionReason(model) {
		return this.http.post<APIResponse>(this.baseUrl + 'v1/Reason/CreateReason', model);
	}

	public deleteExecutionReason(id: number) {
		return this.http.delete<any>(`${this.baseUrl}v1/Reason/DeleteReason/${id}`);
	}
}
