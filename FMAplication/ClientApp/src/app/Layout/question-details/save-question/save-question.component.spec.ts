import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SaveQuestionComponent } from './save-question.component';

describe('SaveQuestionComponent', () => {
  let component: SaveQuestionComponent;
  let fixture: ComponentFixture<SaveQuestionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SaveQuestionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SaveQuestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
