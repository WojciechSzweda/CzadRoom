import { Component, OnInit } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { HostConfig } from 'src/app/host.service'

@Component({
  selector: 'app-public-chat-hub',
  templateUrl: './public-chat-hub.component.html',
  styleUrls: ['./public-chat-hub.component.css']
})
export class PublicChatHubComponent implements OnInit {

  rooms: Object[]
  username: string

  constructor(http: HttpClient, private hostConfig: HostConfig) {
    http.get<PublicRoom[]>(hostConfig.baseURL + 'api/publicchat/rooms').subscribe(result => {
      console.log(result)
      this.rooms = result
    }, error => console.error(error))
    http.post<string>(hostConfig.baseURL + 'api/publicchat/getusername', null).subscribe(result => {
      this.username = result
    }, error => console.error(error))
  }

  ngOnInit() {
  }

}
