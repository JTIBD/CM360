import { RoutesLaout } from "./RoutesLaout";

export class RoutesReports{
        static Parent = RoutesLaout.Reports;    
        static PosmDistributionReport = 'posm-distribution-report';
        static CwStockUpdateReport = 'cw-stock-update-report';
        static SpWisePosmLedgerReport = 'sp-wise-posm-ledger-report';
        static ExecutionReport = 'execution-report';
}