import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DownloadStockDistributionModalComponent } from './download-stock-distribution-modal.component';

describe('DownloadStockDistributionModalComponent', () => {
  let component: DownloadStockDistributionModalComponent;
  let fixture: ComponentFixture<DownloadStockDistributionModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DownloadStockDistributionModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DownloadStockDistributionModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
