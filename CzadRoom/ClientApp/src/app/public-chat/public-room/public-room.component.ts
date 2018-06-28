import { Component, OnInit, Inject, ViewChild, ElementRef, AfterViewChecked, OnDestroy } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { HostConfig } from 'src/app/host.service'
import { ActivatedRoute } from '@angular/router'
import { PublicChatService } from '../public-chat.service'
import { HubConnection } from '@aspnet/signalr'

@Component({
  selector: 'app-public-room',
  templateUrl: './public-room.component.html',
  styleUrls: ['./public-room.component.css']
})
export class PublicRoomComponent implements OnInit, AfterViewChecked, OnDestroy {

  @ViewChild('messageInput') messageInput: ElementRef
  @ViewChild('messagesList') messagesList: ElementRef

  room: Object
  username: string
  users: Object[]
  id: string
  messages: Object[]
  roomHub: HubConnection

  constructor(http: HttpClient, private hostConfig: HostConfig, private route: ActivatedRoute,
    private publicChatService: PublicChatService) {
    route.params.subscribe(params => this.id = params['id'])
    publicChatService.getUsername().subscribe(result => {
      this.username = result
    }, error => console.error(error))
    publicChatService.getRoom(this.id).subscribe(result => {
      this.room = result
    }, error => console.error(error))
  }

  ngOnInit() {
    this.messages = []
    this.roomHub = this.publicChatService.createSignalRConnection()
    this.roomHubCallbacksSetup()
  }

  ngOnDestroy(): void {
    this.roomHub.stop()
  }

  ngAfterViewChecked() {
    this.scrollToBottom()
  }

  sendMessage(message) {
    if (message.length > 0) {
      if (message[0] === '/')
        this.roomHub.invoke('SendCommand', message.substring(1)).catch(err => console.error(err.toString()))
      else
        this.roomHub.invoke('SendRoomMessage', this.id, message).catch(err => console.error(err.toString()))
    }
    this.messageInput.nativeElement.value = ''
  }

  roomHubCallbacksSetup() {
    this.roomHub.on('Connected', () => {
      this.roomHub.invoke('JoinRoom', this.id, this.username).catch(err => console.error(err.toString()))
    })

    this.roomHub.on('ClientJoined', (clientName) => {
      const msg = `${clientName} has joined`
      this.messages.push({user: 'server', content: msg})
    })

    this.roomHub.on('ReceiveMessage', (user, message, _) => {
      this.messages.push({user: user, content: `${user} said ${message}`})
    })
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
