import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LineMemberComponent } from './line-member.component';

describe('LineUserComponent', () => {
  let component: LineMemberComponent;
  let fixture: ComponentFixture<LineMemberComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LineMemberComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LineMemberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
