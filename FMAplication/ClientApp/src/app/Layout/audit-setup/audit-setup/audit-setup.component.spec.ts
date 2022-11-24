import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditSetupComponent } from './audit-setup.component';

describe('AuditSetupComponent', () => {
  let component: AuditSetupComponent;
  let fixture: ComponentFixture<AuditSetupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuditSetupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditSetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
