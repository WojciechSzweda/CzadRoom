import { Injectable } from '@angular/core'
import { HostConfig } from 'src/app/host.service'
import { HttpClient } from '@angular/common/http'
import { HubConnectionBuilder } from '@aspnet/signalr'

@Injectable({
  providedIn: 'root'
})
export class PublicChatService {
  constructor(private hostConfig: HostConfig, private http: HttpClient) {
  }

  public getRooms() {
    return this.http.get<PublicRoom[]>(this.hostConfig.baseURL + 'api/publicchat/rooms')
  }

  public getUsername() {
    return this.http.post<string>(this.hostConfig.baseURL + 'api/publicchat/getusername', null)
  }

  public getRoom(roomId) {
    return this.http.get<PublicRoom>(this.hostConfig.baseURL + 'api/publicchat/room/' + roomId)
  }

  public createSignalRConnection() {
    const connection = new HubConnectionBuilder()
      .withUrl('/publicHub')
      .build()

    connection.start().catch(err => console.error(err.toString()))

    return connection
  }
}
