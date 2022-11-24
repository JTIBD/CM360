import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PosmProductListComponent } from './posm-product-list.component';

describe('PosmProductListComponent', () => {
  let component: PosmProductListComponent;
  let fixture: ComponentFixture<PosmProductListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PosmProductListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PosmProductListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
