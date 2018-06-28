import { Component, OnInit, Inject } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { HostConfig } from '../host.service'
import { ActivatedRoute } from '@angular/router'

@Component({
  selector: 'app-public-room',
  templateUrl: './public-room.component.html',
  styleUrls: ['./public-room.component.css']
})
export class PublicRoomComponent implements OnInit {

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
      this.messages.push({user: 'tester', content: `hello ${i}`})
    }
  }

}
