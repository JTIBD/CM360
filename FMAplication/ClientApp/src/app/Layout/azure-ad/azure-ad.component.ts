import { Component, OnInit } from '@angular/core';
import { BroadcastService, MsalService } from '@azure/msal-angular';
import { Logger, CryptoUtils } from 'msal';

const requestObj = {
    scopes: ['user.read']
};
@Component({
  selector: 'app-azure-ad',
  templateUrl: './azure-ad.component.html',
  styleUrls: ['./azure-ad.component.css']
})
export class AzureAdComponent implements OnInit {
    ngOnInit() { }
    //isIframe = false;
    //loggedIn = false;
    //constructor(
    //    private broadcastService: BroadcastService,
    //    private authService: MsalService
    //) { }

    //// tslint:disable-next-line:use-life-cycle-interface
    //ngOnInit(): void {
    //    this.authService.handleRedirectCallback((authError, response) => {
    //        if (authError) {
    //            console.error('Redirect Error: ', authError.errorMessage);
    //            return;
    //        }

    //        console.log('Redirect Success: ', response);
    //    });
    //    // tslint:disable-next-line:only-arrow-functions
    //    this.authService.acquireTokenSilent(requestObj).then(function (tokenResponse) {
    //        // Callback code here
    //        localStorage.setItem('adtoken', tokenResponse.accessToken);
    //        // tslint:disable-next-line:only-arrow-functions
    //    }).catch(function (error) {
    //        console.log(error);
    //    });
    //    this.authService.setLogger(new Logger((logLevel, message, piiEnabled) => {
    //        console.log('MSAL Logging: ', message);
    //    }, {
    //        correlationId: CryptoUtils.createNewGuid(),
    //        piiLoggingEnabled: false
    //    }));

    //}

}
