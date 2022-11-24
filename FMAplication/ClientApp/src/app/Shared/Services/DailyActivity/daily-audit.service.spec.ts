import { TestBed } from '@angular/core/testing';

import { DailyAuditService } from './daily-audit.service';

describe('DailyAuditService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DailyAuditService = TestBed.get(DailyAuditService);
    expect(service).toBeTruthy();
  });
});
