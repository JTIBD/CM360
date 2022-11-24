import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SalespointTransferComponent } from './salespoint-transfer.component';

describe('SalespointTransferComponent', () => {
  let component: SalespointTransferComponent;
  let fixture: ComponentFixture<SalespointTransferComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SalespointTransferComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SalespointTransferComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
