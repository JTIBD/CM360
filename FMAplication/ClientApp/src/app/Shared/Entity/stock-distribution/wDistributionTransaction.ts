import { PosmProduct, SalesPoint } from "..";

export class WDistributionTransaction{
    id :number;
    transactionId  :number;      
    posmProductId :number;
    posmProductModel: PosmProduct 
    salesPointId :number;
    salesPointViewModel:SalesPoint 
    quantity :number;
}