import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SalespointTransferDetailsComponent } from './salespoint-transfer-details.component';

describe('SalespointTransferDetailsComponent', () => {
  let component: SalespointTransferDetailsComponent;
  let fixture: ComponentFixture<SalespointTransferDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SalespointTransferDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SalespointTransferDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
