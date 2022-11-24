import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerSurveyReportComponent } from './customer-survey-report.component';

describe('CustomerSurveyReportComponent', () => {
  let component: CustomerSurveyReportComponent;
  let fixture: ComponentFixture<CustomerSurveyReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomerSurveyReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerSurveyReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
