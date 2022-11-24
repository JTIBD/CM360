import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalImportPosmAssignComponent } from './modal-import-posm-assign.component';

describe('ModalImportPosmAssignComponent', () => {
  let component: ModalImportPosmAssignComponent;
  let fixture: ComponentFixture<ModalImportPosmAssignComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalImportPosmAssignComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalImportPosmAssignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
