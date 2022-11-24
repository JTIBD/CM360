import {Component, OnInit} from '@angular/core';
import {ThemeOptions} from '../../../../../../theme-options';
import { AzureadService } from '../../../../../../Shared/Services/Azure/azuread.service';
import { LoginService } from '../../../../../../Shared/Services/Users/login.service';
import { BroadcastService, MsalService } from '@azure/msal-angular';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

  const requestObj = {
    scopes: ['user.read']
  };

@Component({
  selector: 'app-user-box',
  templateUrl: './user-box.component.html',
})
export class UserBoxComponent implements OnInit {
    profile;
    adguid: any;
  isIframe = false;
  loggedIn = false;
  production = false;
  constructor(
    public globals: ThemeOptions,
      private adService: AzureadService,
      private broadcastService: BroadcastService,
      private authService: MsalService,
      private loginService: LoginService,
      private router: Router
      
     ) { }

  ngOnInit() {
    this.production = environment.production;
    //this.isIframe = window !== window.parent && !window.opener;
    //// this.login();
    // this.checkoutAccount();

    //   this.broadcastService.subscribe('msal:loginSuccess', () => {
    //     this.checkoutAccount();               
    //   });
      this.getProfile();  
  }

  checkoutAccount() {
    this.loggedIn = !!this.authService.getAccount();
  }

  //login() {
  //  const isIE = window.navigator.userAgent.indexOf('MSIE ') > -1 || window.navigator.userAgent.indexOf('Trident/') > -1;

  //  if (!isIE) {
  //    this.authService.loginRedirect();
  //  } else {
  //    this.authService.loginRedirect();
  //  }
  //}

    getProfile() {
        this.adService.getProfile()
            .toPromise().then(profile => {               
                this.profile = profile;                
            });
    }


  logout() {
    this.authService.logout();
    localStorage.clear();
  }

 

}
