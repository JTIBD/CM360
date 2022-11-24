import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExecutionReasonAddComponent } from './execution-reason-add.component';

describe('ExecutionReasonAddComponent', () => {
  let component: ExecutionReasonAddComponent;
  let fixture: ComponentFixture<ExecutionReasonAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExecutionReasonAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExecutionReasonAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
