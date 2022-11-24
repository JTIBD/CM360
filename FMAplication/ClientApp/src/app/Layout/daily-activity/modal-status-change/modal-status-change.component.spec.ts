import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalStatusChangeComponent } from './modal-status-change.component';

describe('ModalStatusChangeComponent', () => {
  let component: ModalStatusChangeComponent;
  let fixture: ComponentFixture<ModalStatusChangeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalStatusChangeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalStatusChangeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
