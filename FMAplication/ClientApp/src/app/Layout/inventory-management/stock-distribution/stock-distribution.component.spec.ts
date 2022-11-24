import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StockDistributionComponent } from './stock-distribution.component';

describe('StockDistributionComponent', () => {
  let component: StockDistributionComponent;
  let fixture: ComponentFixture<StockDistributionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StockDistributionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StockDistributionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
