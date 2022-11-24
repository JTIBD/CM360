import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExecutionReasonListComponent } from './execution-reason-list.component';

describe('ExecutionReasonListComponent', () => {
  let component: ExecutionReasonListComponent;
  let fixture: ComponentFixture<ExecutionReasonListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExecutionReasonListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExecutionReasonListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
