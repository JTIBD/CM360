import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DailyPosmAddComponent } from './daily-posm-add.component';

describe('DailyPosmAddComponent', () => {
  let component: DailyPosmAddComponent;
  let fixture: ComponentFixture<DailyPosmAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DailyPosmAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DailyPosmAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
