import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompleteReportComponent } from './complete-report.component';

describe('CompleteReportComponent', () => {
  let component: CompleteReportComponent;
  let fixture: ComponentFixture<CompleteReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompleteReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompleteReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
