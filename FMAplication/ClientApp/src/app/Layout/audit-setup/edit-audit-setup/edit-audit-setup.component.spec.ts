import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditAuditSetupComponent } from './edit-audit-setup.component';

describe('EditAuditSetupComponent', () => {
  let component: EditAuditSetupComponent;
  let fixture: ComponentFixture<EditAuditSetupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditAuditSetupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditAuditSetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
