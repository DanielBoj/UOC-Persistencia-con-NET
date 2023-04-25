import { TestBed } from '@angular/core/testing';

import { ReduxService } from './redux.service';

describe('ReduxService', () => {
  let service: ReduxService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ReduxService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
