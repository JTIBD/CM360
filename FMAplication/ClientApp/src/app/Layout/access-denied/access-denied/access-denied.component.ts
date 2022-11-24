import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RoutesDashboard } from 'src/app/Shared/Routes/RoutesDashboard';
import { RoutesLaout } from 'src/app/Shared/Routes/RoutesLaout';

@Component({
  selector: 'app-access-denied',
  templateUrl: './access-denied.component.html',
  styleUrls: ['./access-denied.component.css']
})
export class AccessDeniedComponent implements OnInit {

  constructor(
    private router:Router,
  ) { }

  ngOnInit() {
  }

  handleGoHome(){
    this.router.navigate([RoutesDashboard.Parent,RoutesDashboard.Common]);
  }

}
