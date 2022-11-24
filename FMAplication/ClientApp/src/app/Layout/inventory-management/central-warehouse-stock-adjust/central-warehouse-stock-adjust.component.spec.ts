import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CentralWarehouseStockAdjustComponent } from './central-warehouse-stock-adjust.component';

describe('CentralWarehouseStockAdjustComponent', () => {
  let component: CentralWarehouseStockAdjustComponent;
  let fixture: ComponentFixture<CentralWarehouseStockAdjustComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CentralWarehouseStockAdjustComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CentralWarehouseStockAdjustComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
