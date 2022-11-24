import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddCommunicationSetupComponent } from './add-communication-setup.component';

describe('AddCommunicationSetupComponent', () => {
  let component: AddCommunicationSetupComponent;
  let fixture: ComponentFixture<AddCommunicationSetupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddCommunicationSetupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddCommunicationSetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
