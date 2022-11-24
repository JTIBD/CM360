import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PosmAssignComponent } from './posm-assign.component';

describe('PosmAssignComponent', () => {
  let component: PosmAssignComponent;
  let fixture: ComponentFixture<PosmAssignComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PosmAssignComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PosmAssignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
