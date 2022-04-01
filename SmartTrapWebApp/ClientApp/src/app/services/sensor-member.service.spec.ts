import { TestBed } from '@angular/core/testing';
import { SensorMemberService } from './sensor-member.service';


describe('UserSensorService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SensorMemberService = TestBed.get(SensorMemberService);
    expect(service).toBeTruthy();
  });
});
