import { Component, OnInit } from '@angular/core';
import { AzureadService } from 'src/app/Shared/Services/Azure/azuread.service';
import { MsalService } from '@azure/msal-angular';
import { LoginService } from 'src/app/Shared/Services/Users';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment.prod';
import { Location } from '@angular/common'
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { ActivityPermissionService } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
const requestObj = {
    scopes: ['user.read', 'User.ReadBasic.All']
};

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  adtoken: string;
  adTokenFromCache: boolean;
  profile;
  loggedIn: boolean;
//   error: string;
  error: any;

  constructor(private loginService: LoginService,
    private authService: MsalService,
    private adService: AzureadService,
    private router: Router,
    private commonService: CommonService,
    private location: Location,
    private activityPermissionService: ActivityPermissionService) { }

  ngOnInit() {
    // tslint:disable-next-line:only-arrow-functions
    this.authService.acquireTokenSilent(requestObj).then((tokenResponse) => {
      localStorage.setItem('adtoken', tokenResponse.accessToken);
      this.adtoken = tokenResponse.accessToken;
      this.adTokenFromCache = tokenResponse.fromCache;
      this.getProfile();
      // tslint:disable-next-line:only-arrow-functions
    }).catch(err => {
        console.log(err);
        this.error = err;
    });
  }
  checkoutAccount() {
    this.loggedIn = !!this.authService.getAccount();
}

getProfile() {
    this.adService.getProfile()
        .toPromise().then(profile => {
            this.profile = profile;
            
            if (!this.adTokenFromCache || localStorage.getItem('fmapptoken') == null) {
                this.login()
            }
            else {
             this.activityPermissionService.setActivityPermissionToSession();
             this.router.navigate(['/']);
             // window.location.href = window.location.origin;
              // window.location.href = environment.redirectUri;
            }            

        }).catch(err => {
            console.log(err);
            if (err.status == 401) {
                this.getProfile()
            }
        });
}


login(): any {
    const loginModel = '{ "AdGuid" : "' + this.profile.id + '" , "email": "' + this.profile.givenName + '", "adtoken" : "'+ this.adtoken + '" }';
    console.log("Login Model", loginModel);
    const credentials = loginModel;
    this.loginService.Login(credentials).subscribe(
        (response: any) => {
            if (response.data !== null) {
                var data = response.data;
                const token = data.token;
                localStorage.setItem('fmapptoken', token);
                localStorage.setItem('fmapptoken.expiry', data.expiration);
                this.loginService.getAdUser().subscribe((res: any) => {
                    console.log("User Data.......+++", res.data);
                    // localStorage.setItem('userinfo', JSON.stringify(res.data));
                    this.commonService.setUserInfoToLocalStorage(res.data);
                    this.activityPermissionService.setActivityPermissionToSession();
                    return this.router.navigate(['/']);              
                });
                
            }
            else {
                return this.router.navigate(['/login/unauthorized']);
            }

        },
        error => {

            this.authService.clearCacheForScope(this.adtoken);
            localStorage.clear();
            return this.router.navigate(['/login/unauthorized']);

        }

    );
    }

    Reload() {
        localStorage.clear();
        window.location.href = environment.redirectUri;
    }

}
