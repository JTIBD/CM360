import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SalespointStockAdjustmentListComponent } from './salespoint-stock-adjustment-list.component';

describe('SalespointStockAdjustmentComponent', () => {
  let component: SalespointStockAdjustmentListComponent;
  let fixture: ComponentFixture<SalespointStockAdjustmentListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SalespointStockAdjustmentListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SalespointStockAdjustmentListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
