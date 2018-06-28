import { TestBed, inject } from '@angular/core/testing'

import { PublicChatService } from './public-chat.service'

describe('PublicChatService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PublicChatService]
    })
  })

  it('should be created', inject([PublicChatService], (service: PublicChatService) => {
    expect(service).toBeTruthy()
  }))
})
