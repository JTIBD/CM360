import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExecutionReportComponent } from './execution-report.component';

describe('ExecutionReportComponent', () => {
  let component: ExecutionReportComponent;
  let fixture: ComponentFixture<ExecutionReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExecutionReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExecutionReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
