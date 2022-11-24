import { TestBed } from '@angular/core/testing';

import { DailyPosmService } from './daily-posm.service';

describe('DailyPosmService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DailyPosmService = TestBed.get(DailyPosmService);
    expect(service).toBeTruthy();
  });
});
