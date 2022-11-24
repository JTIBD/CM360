import { Node } from "./node";
import { NodeTree } from "./nodeTree";
import { SalesPoint } from "./salespoint";

export class NodeHieararchy{
    regions:NodeTree[]=[];
    areas:NodeTree[]=[];
    teritories:NodeTree[]=[];
    salespoints:SalesPoint[]=[];
    topLavelHierarchyCode:string;
}