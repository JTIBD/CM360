import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InformationReportComponent } from './information-report.component';

describe('InformationReportComponent', () => {
  let component: InformationReportComponent;
  let fixture: ComponentFixture<InformationReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InformationReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InformationReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
