import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CmTaskGenerationComponent } from './cm-task-generation.component';

describe('CmTaskGenerationComponent', () => {
  let component: CmTaskGenerationComponent;
  let fixture: ComponentFixture<CmTaskGenerationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CmTaskGenerationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CmTaskGenerationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
