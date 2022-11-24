import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewAuditSetupComponent } from './new-audit-setup.component';

describe('NewAuditSetupComponent', () => {
  let component: NewAuditSetupComponent;
  let fixture: ComponentFixture<NewAuditSetupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewAuditSetupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewAuditSetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
