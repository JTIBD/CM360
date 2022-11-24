import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SalespointMultiSelectorComponent } from './salespoint-multi-selector.component';

describe('SalespointMultiSelectorComponent', () => {
  let component: SalespointMultiSelectorComponent;
  let fixture: ComponentFixture<SalespointMultiSelectorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SalespointMultiSelectorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SalespointMultiSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
