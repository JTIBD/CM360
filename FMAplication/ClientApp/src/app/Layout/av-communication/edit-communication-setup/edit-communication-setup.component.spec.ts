import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditCommunicationSetupComponent } from './edit-communication-setup.component';

describe('EditCommunicationSetupComponent', () => {
  let component: EditCommunicationSetupComponent;
  let fixture: ComponentFixture<EditCommunicationSetupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditCommunicationSetupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditCommunicationSetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
