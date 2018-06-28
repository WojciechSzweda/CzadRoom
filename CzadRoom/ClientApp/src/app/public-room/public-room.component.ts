import { Component, OnInit, Inject, ViewChild, ElementRef, AfterViewChecked } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { HostConfig } from '../host.service'
import { ActivatedRoute } from '@angular/router'

@Component({
  selector: 'app-public-room',
  templateUrl: './public-room.component.html',
  styleUrls: ['./public-room.component.css']
})
export class PublicRoomComponent implements OnInit, AfterViewChecked {

  @ViewChild('messageInput') messageInput: ElementRef
  @ViewChild('messagesList') messagesList: ElementRef

  room: Object
  username: string
  users: Object[]
  id: string
  messages: Object[]

  constructor(http: HttpClient, private hostConfig: HostConfig, private route: ActivatedRoute) {
    route.params.subscribe(params => this.id = params['id'])
    http.post<string>(hostConfig.baseURL + 'api/publicchat/getusername', null).subscribe(result => {
      this.username = result
    }, error => console.error(error))
    http.get<PublicRoom>(hostConfig.baseURL + 'api/publicchat/room/' + this.id).subscribe(result => {
      this.room = result
    }, error => console.error(error))
  }

  ngOnInit() {
    this.messages = []
    for (let i = 0; i < 20; i++) {
      this.messages.push({ user: 'tester', content: `hello ${i}` })
    }
  }

  ngAfterViewChecked() {
    this.scrollToBottom()
  }

  sendMessage(message) {
    this.messageInput.nativeElement.value = ''
    this.messages.push({ user: 'tester', content: `${message}` })
  }

  keyDown(event, message) {
    if (event.keyCode === 13 && !event.shiftKey) {
      this.sendMessage(message)
      event.preventDefault()
    }
  }

  scrollToBottom() {
    try {
      this.messagesList.nativeElement.scrollTop = this.messagesList.nativeElement.scrollHeight
    } catch (err) {
      console.error(err)
    }
  }
}
