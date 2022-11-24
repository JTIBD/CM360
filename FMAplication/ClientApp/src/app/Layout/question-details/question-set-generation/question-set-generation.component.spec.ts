import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QuestionSetGenerationComponent } from './question-set-generation.component';

describe('SurveyGenerationComponent', () => {
  let component: QuestionSetGenerationComponent;
  let fixture: ComponentFixture<QuestionSetGenerationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QuestionSetGenerationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuestionSetGenerationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
