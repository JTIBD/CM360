import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PosmProductAddComponent } from './posm-product-add.component';

describe('PosmProductAddComponent', () => {
  let component: PosmProductAddComponent;
  let fixture: ComponentFixture<PosmProductAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PosmProductAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PosmProductAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
