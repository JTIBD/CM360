import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAvCommunicationComponent } from './add-av-communication.component';

describe('AddAvCommunicationComponent', () => {
  let component: AddAvCommunicationComponent;
  let fixture: ComponentFixture<AddAvCommunicationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddAvCommunicationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddAvCommunicationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
