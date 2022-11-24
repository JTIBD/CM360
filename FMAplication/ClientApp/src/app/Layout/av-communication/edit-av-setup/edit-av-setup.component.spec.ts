import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditAvSetupComponent } from './edit-av-setup.component';

describe('EditAvSetupComponent', () => {
  let component: EditAvSetupComponent;
  let fixture: ComponentFixture<EditAvSetupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditAvSetupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditAvSetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
