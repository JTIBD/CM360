import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CentralWarehouseStockAddComponent } from './central-warehouse-stock-add.component';

describe('CentralWarehouseStockAddComponent', () => {
  let component: CentralWarehouseStockAddComponent;
  let fixture: ComponentFixture<CentralWarehouseStockAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CentralWarehouseStockAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CentralWarehouseStockAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
