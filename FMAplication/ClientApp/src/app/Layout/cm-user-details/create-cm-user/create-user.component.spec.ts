import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateCmUserComponent } from './create-user.component';

describe('CreateUserComponent', () => {
  let component: CreateCmUserComponent;
  let fixture: ComponentFixture<CreateCmUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateCmUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateCmUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
