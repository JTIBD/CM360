import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

const GRAPH_ENDPOINT = 'https://graph.microsoft.com/v1.0/';
const GRAPH_ENDPOINT_USERS = 'https://graph.microsoft.com/v1.0/users';
const GRAPH_ENDPOINT_GROUPS = 'https://graph.microsoft.com/v1.0/groups';
const requestObj = {
  scopes: ['user.read']
};
const headers = new HttpHeaders({
  'Authorization': 'Bearer '+localStorage.getItem("adtoken")
});
const headers1 = new HttpHeaders({
  'Authorization': 'Bearer '+localStorage.getItem("msal.idtoken")
});

@Injectable({
  providedIn: 'root'
})
export class AzureadService {

  constructor(private http: HttpClient) { }
  getProfile() {
    return this.http.get(GRAPH_ENDPOINT+ 'me');
}

  getUsers(email: string) {
    let filterText ="?$filter=userPrincipalName eq '"+ email + "'";
  
    return this.http.get(GRAPH_ENDPOINT+ 'users'+ filterText);
  }

  getGroups() {
    return this.http.get(GRAPH_ENDPOINT+ 'groups', {headers});
  }

  checkUserValidity() {
    return !!localStorage.getItem('adtoken') && localStorage.getItem('fmapptoken');
    }

    checkAdTokenAvailability() {
        return !!localStorage.getItem('adtoken');
    }

}
