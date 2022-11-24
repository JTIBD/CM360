export enum TransactionStatus {
    Pending,
    WaitingForApproval, 
    Completed,
    WaitingForReceive
}

export const TransactionStatusStrs:string[]=["Pending","Waiting for approval","Completed","Waiting for receive"]