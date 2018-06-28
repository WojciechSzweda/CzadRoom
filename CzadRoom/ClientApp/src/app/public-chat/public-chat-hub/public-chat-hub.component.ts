import { Component, OnInit } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { HostConfig } from 'src/app/host.service'
import { PublicChatService } from '../public-chat.service'

@Component({
  selector: 'app-public-chat-hub',
  templateUrl: './public-chat-hub.component.html',
  styleUrls: ['./public-chat-hub.component.css']
})
export class PublicChatHubComponent implements OnInit {

  rooms: PublicRoom[]
  username: string

  constructor(private publicChatService: PublicChatService) {
    publicChatService.getRooms().subscribe(result => this.rooms = result, err => console.error(err))
    publicChatService.getUsername().subscribe(result => this.username = result, err => console.error(err))
  }

  ngOnInit() {
  }

}
