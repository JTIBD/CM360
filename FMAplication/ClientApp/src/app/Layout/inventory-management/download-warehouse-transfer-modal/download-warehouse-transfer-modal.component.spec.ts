import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DownloadWarehouseTransferModalComponent } from './download-warehouse-transfer-modal.component';

describe('DownloadWarehouseTransferModalComponent', () => {
  let component: DownloadWarehouseTransferModalComponent;
  let fixture: ComponentFixture<DownloadWarehouseTransferModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DownloadWarehouseTransferModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DownloadWarehouseTransferModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
