import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { LoginService } from '../Services/Users';
import { MsalService } from '@azure/msal-angular';
import { AzureadService } from '../Services/Azure/azuread.service';

const requestObj = {
  scopes: ['user.read']
};


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  profile;
  isIframe = false;
  loggedIn = false;
  public adTokenFromCache = false;
  adtoken: any;
  constructor(
    private loginService: LoginService,
    private authService: MsalService,
    private adService: AzureadService,
    private router: Router
  ) { }
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      if (this.adService.checkUserValidity()) {     
          return true;
      }
      else {       
           this.router.navigate(['/login/login']);
           return false;
      }
  }



  
  checkValidUser() {
    this.loggedIn = !!this.authService.getAccount();
  }

}

