import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditGuidelineSetupComponent } from './edit-guideline-setup.component';

describe('EditGuidelineSetupComponent', () => {
  let component: EditGuidelineSetupComponent;
  let fixture: ComponentFixture<EditGuidelineSetupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditGuidelineSetupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditGuidelineSetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
