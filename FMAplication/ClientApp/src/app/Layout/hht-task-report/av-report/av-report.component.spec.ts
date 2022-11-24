import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AvReportComponent } from './av-report.component';

describe('AvReportComponent', () => {
  let component: AvReportComponent;
  let fixture: ComponentFixture<AvReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AvReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AvReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
