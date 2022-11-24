import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CentralWarehouseStockAdjustmentListComponent } from './central-warehouse-stock-adjustment-list.component';

describe('CentralWarehouseStockAdjustmentListComponent', () => {
  let component: CentralWarehouseStockAdjustmentListComponent;
  let fixture: ComponentFixture<CentralWarehouseStockAdjustmentListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CentralWarehouseStockAdjustmentListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CentralWarehouseStockAdjustmentListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
