import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewGuidelineSetupComponent } from './new-guideline-setup.component';

describe('NewGuidelineSetupComponent', () => {
  let component: NewGuidelineSetupComponent;
  let fixture: ComponentFixture<NewGuidelineSetupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewGuidelineSetupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewGuidelineSetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
