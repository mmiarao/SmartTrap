import { TestBed } from '@angular/core/testing';

import { WebApiClientService } from './web-api-client.service';

describe('WebApiClientService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: WebApiClientService = TestBed.get(WebApiClientService);
    expect(service).toBeTruthy();
  });
});
