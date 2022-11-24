import { Router } from '@angular/router';
import { MinimumExecutionLimitService } from './../../../Shared/Services/minimum-execution-limit/minimum-execution-limit.service';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { UserService } from './../../../Shared/Services/Users/user.service';
import { Component, OnInit } from '@angular/core';
import { IDropdown } from 'src/app/Shared/interfaces/IDropdown';
import { MinimumExecutionLimit, MinimumExecutionLimitTableData } from 'src/app/Shared/Entity/minimum-execution-limit/minimum-execution-limit';
import { NodeTree, SalesPoint } from 'src/app/Shared/Entity';
import { first } from 'rxjs/operators';
import { RoutesExecutionLimit } from 'src/app/Shared/Routes/RoutesExecutionLimit';

@Component({
  selector: 'app-execution-limit-add',
  templateUrl: './execution-limit-add.component.html',
  styleUrls: ['./execution-limit-add.component.css']
})
export class ExecutionLimitAddComponent implements OnInit {

  pageIndex = 1;
  pageSize = 10;
  total:number;
  salesPointDropdownData : IDropdown[] = [];
  minimumExecutionLimits: MinimumExecutionLimit[] = [];
  minimumExecutionLimitTableData: MinimumExecutionLimitTableData[] = [];
  search:string = "";
  nodeTree: NodeTree[] = [];
  
  targetVisitedOutlet: number;
  salesPointIds: number[] = [];

  constructor(private userService: UserService, private alertService: AlertService,
    private minimumExecutionLimitService: MinimumExecutionLimitService, private router: Router) { }

  ngOnInit() {
    this.userService.getNodeTreeByCurrentUser().subscribe(data => {
      this.nodeTree = data;
    })
  }

  submit(){
    if(this.targetVisitedOutlet < 0){
      this.alertService.tosterDanger("Target visited outlet can not be negative");
      return;
    }
    const minimumExecutionLimitList:MinimumExecutionLimit[]=[];
    const salesPoints = this.getSelectedSalesPoints();

    if(!salesPoints.length) {
      this.alertService.titleTosterDanger("No salespoint selected");
      return;
    }
    salesPoints.forEach(sp=>{
      const ml = new MinimumExecutionLimit();
      ml.code="M_"+sp.code;
      ml.salesPointId = sp.salesPointId;
      ml.targetVisitedOutlet = this.targetVisitedOutlet;
      minimumExecutionLimitList.push(ml);
    });
    this.minimumExecutionLimitService.getExistingMinimumExecutionLimit({data:minimumExecutionLimitList}).pipe(first()).subscribe(res=>{
      if(res.length) {
        if(this.alertService.loadingFlag) this.alertService.fnLoading(false);
        let sps = salesPoints.filter(x=> res.some(sv=>sv.salesPointId == x.salesPointId)).filter((sp,i,arr)=> arr.findIndex(x=>x.salesPointId == sp.salesPointId) === i);
        let spNames = sps.map(x=>x.name);
        this.alertService.confirm(`Target Visited Outlet already exist in Salespoint ${spNames.join(", ")}. Do you want to Update it?`, () => {
          
          this.minimumExecutionLimitService.createNewMinimumExecutionLimit({data:minimumExecutionLimitList}).subscribe(res => {
            console.log(res);
            this.alertService.tosterSuccess("Target Visited Outlet created successfully");
            this.router.navigate(["/configuration/execution-limit"]);
          });
        },()=>{}); 

      }
      else{
        this.minimumExecutionLimitService.createNewMinimumExecutionLimit({ data: minimumExecutionLimitList }).subscribe(res => {
          console.log(res);
          this.alertService.tosterSuccess("Target Visited Outlet created successfully");
          this.router.navigate(["/configuration/execution-limit"]);
        });
      }
    })

  }

  handleSalesPointSelect(event, salesPoint: SalesPoint) {
    salesPoint.isSelected = event.target.checked;
  }

  getSelectedSalesPoints(){
    const salesPointIds:SalesPoint[]=[];
    let fun=(tree:NodeTree[])=>{
      tree.forEach(tr=>{
        if(!!tr.salesPoints && !!tr.salesPoints.length){
          tr.salesPoints.forEach(x=>{
            if(x.isSelected) salesPointIds.push(x);
          })
        }
        else if(!!tr.nodes) fun(tr.nodes);
      })
    }
    fun(this.nodeTree);
    return  salesPointIds;
  }


  handleNodeSelect(item: NodeTree, checked: boolean) {
    console.log(item, checked);
    const node: NodeTree = this.getNodeById(item.node.id);
    console.log(node);
    if (!node) return;
    let fun = (trees: NodeTree[], checked: boolean) => {
      trees.forEach(tr => {
        tr.isSelected = checked;
        if (!!tr.nodes && tr.nodes.length) fun(tr.nodes, checked);
        else if (!!tr.salesPoints && !!tr.salesPoints.length) {
          tr.salesPoints.forEach(sl => {
            sl.isSelected = checked;
          })
        }
      })
    }
    fun([node], checked);
  }

  getNodeById(id: number) {
    let find = (tree: NodeTree[]) => {
      let node = tree.find(t => t.node.id === id);
      if (node) return node;
      //@ts-ignore
      else return find(tree.filter(x => !!x.nodes).map(x => x.nodes).flat());
    }
    return find(this.nodeTree);

  }
  handleBack(){
    this.router.navigate([RoutesExecutionLimit.Parent,RoutesExecutionLimit.MinimumExecutionLimit]);
  }
}
