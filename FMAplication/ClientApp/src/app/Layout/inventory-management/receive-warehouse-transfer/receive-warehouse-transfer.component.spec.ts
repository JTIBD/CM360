import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceiveWarehouseTransferComponent } from './receive-warehouse-transfer.component';

describe('ReceiveWarehouseTransferComponent', () => {
  let component: ReceiveWarehouseTransferComponent;
  let fixture: ComponentFixture<ReceiveWarehouseTransferComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReceiveWarehouseTransferComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReceiveWarehouseTransferComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
