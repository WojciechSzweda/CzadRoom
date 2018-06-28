import { async, ComponentFixture, TestBed } from '@angular/core/testing'

import { DirectMessagesHubComponent } from './direct-messages-hub.component'

describe('DirectMessagesHubComponent', () => {
  let component: DirectMessagesHubComponent
  let fixture: ComponentFixture<DirectMessagesHubComponent>

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DirectMessagesHubComponent ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(DirectMessagesHubComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
