import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditCmUserComponent } from './edit-user.component';

describe('EditUserComponent', () => {
  let component: EditCmUserComponent;
  let fixture: ComponentFixture<EditCmUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditCmUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditCmUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
