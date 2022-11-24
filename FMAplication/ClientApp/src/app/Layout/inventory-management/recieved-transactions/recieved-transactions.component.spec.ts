import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RecievedTransactionsComponent } from './recieved-transactions.component';

describe('RecievedTransactionsComponent', () => {
  let component: RecievedTransactionsComponent;
  let fixture: ComponentFixture<RecievedTransactionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RecievedTransactionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RecievedTransactionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
