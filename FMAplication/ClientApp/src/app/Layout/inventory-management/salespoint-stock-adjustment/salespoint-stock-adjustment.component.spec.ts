import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SalespointStockAdjustmentComponent } from './salespoint-stock-adjustment.component';

describe('SalespointStockAdjustmentComponent', () => {
  let component: SalespointStockAdjustmentComponent;
  let fixture: ComponentFixture<SalespointStockAdjustmentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SalespointStockAdjustmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SalespointStockAdjustmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
