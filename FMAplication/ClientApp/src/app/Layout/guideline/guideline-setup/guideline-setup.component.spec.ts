import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GuidelineSetupComponent } from './guideline-setup.component';

describe('GuidelineSetupComponent', () => {
  let component: GuidelineSetupComponent;
  let fixture: ComponentFixture<GuidelineSetupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GuidelineSetupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GuidelineSetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
