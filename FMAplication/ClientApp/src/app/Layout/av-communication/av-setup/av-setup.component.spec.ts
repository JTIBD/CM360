import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AvSetupComponent } from './av-setup.component';

describe('AvSetupComponent', () => {
  let component: AvSetupComponent;
  let fixture: ComponentFixture<AvSetupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AvSetupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AvSetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
