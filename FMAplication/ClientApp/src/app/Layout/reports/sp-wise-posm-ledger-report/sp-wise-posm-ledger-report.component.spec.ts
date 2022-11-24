import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SpWisePosmLedgerReportComponent } from './sp-wise-posm-ledger-report.component';

describe('SpWisePosmLedgerReportComponent', () => {
  let component: SpWisePosmLedgerReportComponent;
  let fixture: ComponentFixture<SpWisePosmLedgerReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SpWisePosmLedgerReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SpWisePosmLedgerReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
