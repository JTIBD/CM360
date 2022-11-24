import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DailyAuditListComponent } from './daily-audit-list.component';

describe('DailyAuditListComponent', () => {
  let component: DailyAuditListComponent;
  let fixture: ComponentFixture<DailyAuditListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DailyAuditListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DailyAuditListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
