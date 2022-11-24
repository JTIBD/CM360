	import { Component, OnInit } from '@angular/core';
	import { Reason } from 'src/app/Shared/Entity/ExecutionReason/ExecutionReason';
	import { ActivatedRoute, Router } from '@angular/router';
	import { AlertService } from "../../../Shared/Modules/alert/alert.service";
	import { MapObject } from "../../../Shared/Enums/mapObject";
	import { StatusTypes } from "../../../Shared/Enums/statusTypes";
	import { ExecutionReasonService } from 'src/app/Shared/Services/Reasons/execution-reason.service';
import { ReasonType } from 'src/app/Shared/Entity/ExecutionReason/ReasonType';
import { ReasonReasonTypeMapping } from 'src/app/Shared/Entity/ExecutionReason/ReasonReasonTypeMapping';

	@Component({
	selector: 'app-execution-reason-add',
	templateUrl: './execution-reason-add.component.html',
	styleUrls: ['./execution-reason-add.component.css']
	})
	export class ExecutionReasonAddComponent implements OnInit {
	public reasonModel: Reason = new Reason();
	enumStatusTypes: MapObject[] = StatusTypes.statusType.slice(0,2);
	reasonTypes:ReasonType[]=[];
	selectedReasonTypeIds:number[]=[];

	constructor(private alertService: AlertService, private route: ActivatedRoute, private reasonService: ExecutionReasonService, private router: Router) { }

	ngOnInit() {
		this.createForm();
		console.log("param", this.route.snapshot.params, Object.keys(this.route.snapshot.params).length);

		if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
			console.log("id", this.route.snapshot.params.id);
			let reasonId = this.route.snapshot.params.id;
			this.getExecutionReason(reasonId);
		}
		this.getReasonTypes();
	}

	getReasonTypes(){
		this.reasonService.getReasonTypes().subscribe(res=>{
			this.reasonTypes = res;
			if(this.reasonTypes) this.reasonTypes = this.reasonTypes.filter(x=>x.text != "Av");
		})
	}

	public changeReason(form: any) {
		console.log(form);
		if (!form.value.reason && !form.value.reasonBangla) {
			form.errors = { noDescription: true };
		}
		else {
			form.errors = null;
		}
	}

	createForm() {
		// console.log("statusOptions: " , this.statusOptions, this.keys);
	}

	public fnRouteExecutionReasonList() {
		this.router.navigate(['/configuration/execution-reasons/execution-reason-list']);
	}

	private getExecutionReason(reasonId) {
		this.reasonService.getReason(reasonId).subscribe(
			(result) => {
				this.selectedReasonTypeIds = result.data.reasonReasonTypeMappings.map(x=>x.reasonTypeId);
				this.editForm(result.data);
			},
			(err: any) => console.log(err)
		);
	};

	private editForm(reason: Reason) {
		this.reasonModel.id = reason.id;
		this.reasonModel.name = reason.name;
		this.reasonModel.reasonInEnglish = reason.reasonInEnglish;
		this.reasonModel.reasonInBangla = reason.reasonInBangla;
		this.reasonModel.status = reason.status;
		console.log("posm product data edit after", this.reasonModel);
	}

	public fnSaveExecutionReason(model: Reason) {
		
		this.reasonModel.reasonReasonTypeMappings = this.selectedReasonTypeIds.map(x=> {
			let mapp = new ReasonReasonTypeMapping();
			mapp.reasonTypeId = x
			return mapp;
		});
		this.reasonModel.id ? this.updateExecutionReason(this.reasonModel) : this.insertExecutionReason(this.reasonModel);
	}

	private insertExecutionReason(model: Reason) {
		this.reasonService.createExecutionReason(model).subscribe(res => {
			console.log("ExecutionReason res: ", res);
			this.router.navigate(['/configuration/execution-reasons/execution-reason-list']).then(() => {
				this.alertService.tosterSuccess("Execution Reasons has been created successfully.");
			});
		},
			(error) => {
				console.log(error);
				this.alertService.tosterDanger(error.statusText);
			}, () => this.alertService.fnLoading(false)
		);
	}

	private updateExecutionReason(model: Reason) {
		this.reasonService.updateExecutionReason(model).subscribe(res => {
			console.log("Execution Reason upd res: ", res);
			this.router.navigate(['/configuration/execution-reasons/execution-reason-list']).then(() => {
				this.alertService.tosterSuccess("Execution Reasons has been edited successfully.");
			});
		},
			(error) => {
				console.log(error);
				this.alertService.tosterDanger(error.statusText);
			}, () => this.alertService.fnLoading(false)
		);
	}

}
