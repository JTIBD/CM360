import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CmTaskListComponent } from './cm-task-list.component';

describe('CmTaskListComponent', () => {
  let component: CmTaskListComponent;
  let fixture: ComponentFixture<CmTaskListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CmTaskListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CmTaskListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
