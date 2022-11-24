import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalSKUPickerComponent } from './modal-skupicker.component';

describe('ModalSKUPickerComponent', () => {
  let component: ModalSKUPickerComponent;
  let fixture: ComponentFixture<ModalSKUPickerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalSKUPickerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalSKUPickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
