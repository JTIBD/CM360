import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RecieveStockComponent } from './recieve-stock.component';

describe('RecieveStockComponent', () => {
  let component: RecieveStockComponent;
  let fixture: ComponentFixture<RecieveStockComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RecieveStockComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RecieveStockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
