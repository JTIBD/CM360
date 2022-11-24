import { TestBed } from '@angular/core/testing';

import { FileUtilityService } from './file-utility.service';

describe('FileUtilityService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: FileUtilityService = TestBed.get(FileUtilityService);
    expect(service).toBeTruthy();
  });
});
