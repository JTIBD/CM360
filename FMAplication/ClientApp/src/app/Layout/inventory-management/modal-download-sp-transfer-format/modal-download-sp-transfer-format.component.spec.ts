import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalDownloadSpTransferFormatComponent } from './modal-download-sp-transfer-format.component';

describe('ModalDownloadSpTransferFormatComponent', () => {
  let component: ModalDownloadSpTransferFormatComponent;
  let fixture: ComponentFixture<ModalDownloadSpTransferFormatComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalDownloadSpTransferFormatComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalDownloadSpTransferFormatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
