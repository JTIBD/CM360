import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DownloadStockModalComponent } from './download-stock-modal.component';

describe('DownloadStockModalComponent', () => {
  let component: DownloadStockModalComponent;
  let fixture: ComponentFixture<DownloadStockModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DownloadStockModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DownloadStockModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
