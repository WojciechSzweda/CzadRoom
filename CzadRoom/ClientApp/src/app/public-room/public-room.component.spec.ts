import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PublicRoomComponent } from './public-room.component';

describe('PublicRoomComponent', () => {
  let component: PublicRoomComponent;
  let fixture: ComponentFixture<PublicRoomComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PublicRoomComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PublicRoomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
