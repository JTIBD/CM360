import { TestBed } from '@angular/core/testing';

import { InventoryManagementService } from './inventory-management.service';

describe('InventoryManagementService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: InventoryManagementService = TestBed.get(InventoryManagementService);
    expect(service).toBeTruthy();
  });
});
