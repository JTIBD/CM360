import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DailyAuditAddComponent } from './daily-audit-add.component';

describe('DailyAuditAddComponent', () => {
  let component: DailyAuditAddComponent;
  let fixture: ComponentFixture<DailyAuditAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DailyAuditAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DailyAuditAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
