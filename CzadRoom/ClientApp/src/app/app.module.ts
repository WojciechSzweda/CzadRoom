import { BrowserModule } from '@angular/platform-browser'
import { NgModule } from '@angular/core'
import { RouterModule } from '@angular/router'
import { AppMaterialModule } from './app-material/app-material.module'
import { HttpClientModule } from '@angular/common/http'

import { AppComponent } from './app.component'

import 'hammerjs'
import { ChatRoomComponent } from './chat-room/chat-room.component'
import { PublicRoomComponent } from './public-chat/public-room/public-room.component'
import { FriendsComponent } from './friends/friends.component'
import { HomeComponent } from './home/home.component'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { AppNavComponent } from './app-nav/app-nav.component'
import { LayoutModule } from '@angular/cdk/layout'
import { PublicChatHubComponent } from './public-chat/public-chat-hub/public-chat-hub.component'
import { DirectMessagesHubComponent } from './direct-messages/direct-messages-hub/direct-messages-hub.component'
import { DirectMessageRoomComponent } from './direct-messages/direct-message-room/direct-message-room.component'

@NgModule({
  declarations: [
    AppComponent,
    ChatRoomComponent,
    PublicRoomComponent,
    FriendsComponent,
    HomeComponent,
    AppNavComponent,
    PublicChatHubComponent,
    DirectMessagesHubComponent,
    DirectMessageRoomComponent,
  ],
  imports: [
    BrowserModule,
    AppMaterialModule,
    RouterModule.forRoot([
      { path: '', redirectTo: '/home', pathMatch: 'full' },
      { path: 'home', component: HomeComponent },
      { path: 'chat', component: ChatRoomComponent },
      { path: 'public', component: PublicChatHubComponent },
      { path: 'messages', component: DirectMessagesHubComponent },
      { path: 'friends', component: FriendsComponent },
      { path: 'public/room/:id', component: PublicRoomComponent },
    ]),
    BrowserAnimationsModule,
    LayoutModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
