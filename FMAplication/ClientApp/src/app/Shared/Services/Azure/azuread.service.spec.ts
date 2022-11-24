import { TestBed } from '@angular/core/testing';

import { AzureadService } from './azuread.service';

describe('AzureadService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AzureadService = TestBed.get(AzureadService);
    expect(service).toBeTruthy();
  });
});
