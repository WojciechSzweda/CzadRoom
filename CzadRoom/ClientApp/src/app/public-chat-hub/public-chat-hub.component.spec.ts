import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PublicChatHubComponent } from './public-chat-hub.component';

describe('PublicChatHubComponent', () => {
  let component: PublicChatHubComponent;
  let fixture: ComponentFixture<PublicChatHubComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PublicChatHubComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PublicChatHubComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
