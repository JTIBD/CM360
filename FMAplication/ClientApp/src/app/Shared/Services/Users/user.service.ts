﻿import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse, NodeTree, SalesPoint } from '../../Entity';
import { CommonService } from '../Common/common.service';

@Injectable({
    providedIn: 'root'
})

export class UserService {
    public baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string,
        private commonService: CommonService) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }
    /////////////////////CM User///////////

    getUserList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/user');
    }
    getUserById(id) {
        return this.http.get(`${this.baseUrl}v1/user/${id}`);
    }
    createUser(userModel) {
        console.log(userModel);
        return this.http.post(this.baseUrl + 'v1/user/create', userModel);
    }
    updateUser(userModel) {
        console.log(userModel);
        return this.http.put(this.baseUrl + 'v1/user/update', userModel);

    }
    deleteUser(id) {
        return this.http.delete(`${this.baseUrl}v1/user/delete/${id}`);
    }
    excelImportUser(model) {
        return this.http.post(`${this.baseUrl}v1/user/excelImport`, this.commonService.toFormData(model));
    }

    //public postUser(model) {
    //    return this.http.post<APIResponse>(this.baseUrl + 'v1/user/save', model);
    //}

    //public postRoleLinkWithUser(model) {
    //    return this.http.post<APIResponse>(this.baseUrl + 'v1/user/rolelinkwithuser', model);
    //}



    /////////////////User Info /////////////
createUserInfo(userInfoModel) {
        console.log(userInfoModel);
        return this.http.post(this.baseUrl + 'v1/userinfo/create', userInfoModel);
    }
    updateUserInfo(userInfoModel) {
        console.log(userInfoModel);
        return this.http.put(this.baseUrl + 'v1/userinfo/update', userInfoModel);

    }
    getAllUserInfo() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/userinfo');
    }
    getAllSalesPointByCurrentUser() {
        return this.http.get<APIResponse<SalesPoint[]>>(this.baseUrl + 'v1/SalesPoint/GetAllSalesPointByCurrentFmUser');
    }//nodeHierarchies

    getNodeTreeByCurrentUser() {
        return this.http.get<NodeTree[]>(this.baseUrl + 'v1/Node/GetNodeTreeByCurrentFmUser');
    }//nodeHierarchies


    getUserInfoById(id) {
        return this.http.get(`${this.baseUrl}v1/userinfo/${id}`);
    }

    getDesignationCodeById(id) {
        return this.http.get(`${this.baseUrl}v1/userinfo/designationcode/${id}`);
    }

    deleteUserInfo(id) {

        return this.http.delete<any>(`${this.baseUrl}v1/userinfo/delete/${id}`);
    }

    public postUser(model) {
        return this.http.post<APIResponse>(this.baseUrl + 'v1/userinfo/save', model);
    }

    public postRoleLinkWithUser(model) {
        return this.http.post<APIResponse>(this.baseUrl + 'v1/userinfo/rolelinkwithuser', model);
    }

    /////////////////Hierarchy /////////////

    getAllHierarchy() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/userinfo/hierarchy');
    }

    getUserTypeCode(userTypeId:number) { 
        return this.http.get<APIResponse>(this.baseUrl + 'v1/user/GetUserTypeCode?userTypeId=' + userTypeId);
    }
}