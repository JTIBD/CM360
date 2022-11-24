import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PosmReportComponent } from './posm-report.component';

describe('PosmReportComponent', () => {
  let component: PosmReportComponent;
  let fixture: ComponentFixture<PosmReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PosmReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PosmReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
