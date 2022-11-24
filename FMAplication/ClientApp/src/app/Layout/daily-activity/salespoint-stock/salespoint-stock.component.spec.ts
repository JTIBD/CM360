import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SalespointStockComponent } from './salespoint-stock.component';

describe('SalespointStockComponent', () => {
  let component: SalespointStockComponent;
  let fixture: ComponentFixture<SalespointStockComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SalespointStockComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SalespointStockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
