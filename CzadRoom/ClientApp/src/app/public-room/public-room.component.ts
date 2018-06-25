import { Component, OnInit, Inject } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import {HostConfig} from '../host.service'

@Component({
  selector: 'app-public-room',
  templateUrl: './public-room.component.html',
  styleUrls: ['./public-room.component.css']
})
export class PublicRoomComponent implements OnInit {

  rooms: Object[]
  username: string

  constructor(http: HttpClient, private hostConfig: HostConfig) {
    http.get<PublicRoom[]>(hostConfig.baseURL + 'api/publicchat/rooms').subscribe(result => {
      this.rooms = result
    }, error => console.error(error))
    http.post<string>(hostConfig.baseURL + 'api/publicchat/getusername', null).subscribe(result => {
      this.username = result
    })
  }

  ngOnInit() {
  }

}