import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseReceivedTransfersComponent } from './warehouse-received-transfers.component';

describe('WarehouseReceivedTransfersComponent', () => {
  let component: WarehouseReceivedTransfersComponent;
  let fixture: ComponentFixture<WarehouseReceivedTransfersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WarehouseReceivedTransfersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseReceivedTransfersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
