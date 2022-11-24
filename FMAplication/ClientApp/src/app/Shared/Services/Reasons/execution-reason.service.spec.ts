import { TestBed } from '@angular/core/testing';

import { ExecutionReasonService } from './execution-reason.service';

describe('ExecutionReasonService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ExecutionReasonService = TestBed.get(ExecutionReasonService);
    expect(service).toBeTruthy();
  });
});
