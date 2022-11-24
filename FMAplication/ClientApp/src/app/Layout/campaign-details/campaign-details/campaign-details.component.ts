import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { finalize } from 'rxjs/operators';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { Campaign } from 'src/app/Shared/Entity/Campaigns';
import { CampaignService } from 'src/app/Shared/Services/Campaign/campaign.service';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';

@Component({
  selector: 'app-campaign-details',
  templateUrl: './campaign-details.component.html',
  styleUrls: ['./campaign-details.component.css']
})
export class CampaignDetailsComponent implements OnInit, OnDestroy {

  enumStatusTypes: MapObject[] = StatusTypes.statusType;
  private subscriptions: Subscription = new Subscription();
  public campaign: Campaign = new Campaign();
  public id: any;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private alertService: AlertService,
    private campaignService: CampaignService,
		private commonService: CommonService
  ) { }

  ngOnInit() {
    this.id = this.activatedRoute.snapshot.params['id'];
    if(this.id){
      this.loadDetails();
    }
    else{
      this.goBack();
    }
  }

  ngOnDestroy() {
		this.subscriptions.unsubscribe();
  }

  loadDetails(){
    this.alertService.fnLoading(true);
		const productsSubscription = this.campaignService.getCampaign(this.id)
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
        (res) => {
          let campaignData = res.data;

          if(campaignData.campaignHistories && campaignData.campaignHistories.length > 0){
            campaignData.campaignHistories.forEach(obj => {
              obj.statusText = this.enumStatusTypes.filter(k => k.id == obj.status)[0].label;
              obj.startDateText = this.dateToString(obj.startDate);
              obj.endDateText = this.dateToString(obj.endDate);
            });
          }

          this.campaign = campaignData;
          console.log('data - ', this.campaign);
        },
        (error) => {
            console.log(error);
        });
			this.subscriptions.add(productsSubscription);
  }

  public ptableSettings: IPTableSetting = {
    tableID: "campaign-history-table",
    tableClass: "table table-border ",
    tableName: 'Campaign History',
    tableRowIDInternalName: "id",
    tableColDef: [
      { headerName: 'Start Date', width: '40%', internalName: 'startDateText', type: "" },
      { headerName: 'End Date', width: '40%', internalName: 'endDateText', type: "" },
      { headerName: 'Status', width: '20%', internalName: 'statusText', type: "" }
    ],
    enabledPagination: false,
    // enabledEditBtn: true,
    enabledColumnFilter: false,
    tableHeaderVisibility: false,
    enabledRadioBtn: false,
    enabledCellClick: true,
    tableFooterVisibility: false
  };

  goBack(){
    this.router.navigate([`/campaign/campaign-list`]);
  }

  dateToString(date: Date): string {
      let newDate = new Date(date);
      return `${newDate.getFullYear()}-${newDate.getMonth()+1}-${newDate.getDate()}`;
  }

  statusText(status: number): string {
      return this.enumStatusTypes.filter(k => k.id == status)[0].label;
  }
}
