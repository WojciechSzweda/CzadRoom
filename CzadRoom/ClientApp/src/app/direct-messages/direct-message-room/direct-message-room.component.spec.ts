import { async, ComponentFixture, TestBed } from '@angular/core/testing'

import { DirectMessageRoomComponent } from './direct-message-room.component'

describe('DirectMessageRoomComponent', () => {
  let component: DirectMessageRoomComponent
  let fixture: ComponentFixture<DirectMessageRoomComponent>

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DirectMessageRoomComponent ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(DirectMessageRoomComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
