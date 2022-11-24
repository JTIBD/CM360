import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DailyPosmListComponent } from './daily-posm-list.component';

describe('DailyPosmListComponent', () => {
  let component: DailyPosmListComponent;
  let fixture: ComponentFixture<DailyPosmListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DailyPosmListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DailyPosmListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
