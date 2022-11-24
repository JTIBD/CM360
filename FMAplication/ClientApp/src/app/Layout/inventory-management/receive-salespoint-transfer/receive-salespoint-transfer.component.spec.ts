import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceiveSalespointTransferComponent } from './receive-salespoint-transfer.component';

describe('ReceiveSalespointTransferComponent', () => {
  let component: ReceiveSalespointTransferComponent;
  let fixture: ComponentFixture<ReceiveSalespointTransferComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReceiveSalespointTransferComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReceiveSalespointTransferComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
