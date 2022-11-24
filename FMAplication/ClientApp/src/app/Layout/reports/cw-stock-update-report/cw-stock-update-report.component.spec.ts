import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CwStockUpdateReportComponent } from './cw-stock-update-report.component';

describe('CwStockUpdateReportComponent', () => {
  let component: CwStockUpdateReportComponent;
  let fixture: ComponentFixture<CwStockUpdateReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CwStockUpdateReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CwStockUpdateReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
