import { TransactionType } from './../../Enums/TransactionType';
import { Transaction } from 'src/app/Shared/Entity/Inventory/Transaction';
import { TWStatus } from '../../Enums/TWStatus';


export class Notification {
  count:number; 
  transactionNotifications:TransactionNotification[]
}
export class TransactionNotification {
    id: number
    transactionId: number
    transaction: Transaction
    transactionWorkFlowId: number
    transactionType : TransactionType
    isSeen: boolean
    twStatus:TWStatus
    submittedBy: string
  }
  
