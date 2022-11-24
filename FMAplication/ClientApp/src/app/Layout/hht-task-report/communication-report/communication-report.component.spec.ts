import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommunicationReportComponent } from './communication-report.component';

describe('CommunicationReportComponent', () => {
  let component: CommunicationReportComponent;
  let fixture: ComponentFixture<CommunicationReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommunicationReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommunicationReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
