import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WareHouseTransferComponent } from './ware-house-transfer.component';

describe('WareHouseTransferComponent', () => {
  let component: WareHouseTransferComponent;
  let fixture: ComponentFixture<WareHouseTransferComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WareHouseTransferComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WareHouseTransferComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
