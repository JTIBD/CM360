import { Selectable } from "../Common/selectable";
import { Node } from "./node";
import { SalesPoint } from "./salespoint";

export class NodeTree extends Selectable{
    node:Node;
    salesPoints?:SalesPoint[];
    nodes?: NodeTree[];
    hierarchyCode?:string;
}