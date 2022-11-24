import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsumerSurveyReportComponent } from './consumer-survey-report.component';

describe('ConsumerSurveyReportComponent', () => {
  let component: ConsumerSurveyReportComponent;
  let fixture: ComponentFixture<ConsumerSurveyReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConsumerSurveyReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConsumerSurveyReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
