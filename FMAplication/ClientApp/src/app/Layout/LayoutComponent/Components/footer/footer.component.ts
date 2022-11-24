import { Component, OnInit } from '@angular/core';
import buildInfo  from "../../../../../build";
import * as moment from 'moment';
import { AppVersion } from 'src/app/Shared/Utility/version';
@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
})
export class FooterComponent implements OnInit {


  currentYear:number;
  lastRelease:any;
  version = AppVersion;

  constructor() {
    this.currentYear = new Date().getFullYear();
    this.lastRelease =  moment(buildInfo.timestamp).format('DD.MM.YYYY.hh.mm A');

   }

 
  ngOnInit() {
  }

}
