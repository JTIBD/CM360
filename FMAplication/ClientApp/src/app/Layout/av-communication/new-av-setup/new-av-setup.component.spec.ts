import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewAvSetupComponent } from './new-av-setup.component';

describe('NewAvSetupComponent', () => {
  let component: NewAvSetupComponent;
  let fixture: ComponentFixture<NewAvSetupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewAvSetupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewAvSetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
