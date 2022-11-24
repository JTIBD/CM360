import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PosmDistributionReportComponent } from './posm-distribution-report.component';

describe('PosmDistributionReportComponent', () => {
  let component: PosmDistributionReportComponent;
  let fixture: ComponentFixture<PosmDistributionReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PosmDistributionReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PosmDistributionReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
