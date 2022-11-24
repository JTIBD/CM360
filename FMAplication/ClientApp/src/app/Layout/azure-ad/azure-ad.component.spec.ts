import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AzureAdComponent } from './azure-ad.component';

describe('AzureAdComponent', () => {
  let component: AzureAdComponent;
  let fixture: ComponentFixture<AzureAdComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AzureAdComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AzureAdComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
