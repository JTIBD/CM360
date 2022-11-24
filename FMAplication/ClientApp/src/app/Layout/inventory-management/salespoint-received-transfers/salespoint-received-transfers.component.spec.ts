import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SalespointReceivedTransfersComponent } from './salespoint-received-transfers.component';

describe('SalespointReceivedTransfersComponent', () => {
  let component: SalespointReceivedTransfersComponent;
  let fixture: ComponentFixture<SalespointReceivedTransfersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SalespointReceivedTransfersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SalespointReceivedTransfersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
