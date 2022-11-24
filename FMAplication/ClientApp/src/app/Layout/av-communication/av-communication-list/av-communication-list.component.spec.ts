import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AvCommunicationListComponent } from './av-communication-list.component';

describe('AvCommunicationListComponent', () => {
  let component: AvCommunicationListComponent;
  let fixture: ComponentFixture<AvCommunicationListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AvCommunicationListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AvCommunicationListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
