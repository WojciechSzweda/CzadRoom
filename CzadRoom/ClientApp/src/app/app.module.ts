import { BrowserModule } from '@angular/platform-browser'
import { NgModule } from '@angular/core'
import { RouterModule } from '@angular/router'
import { AppMaterialModule } from './app-material/app-material.module'
import { HttpClientModule } from '@angular/common/http'

import { AppComponent } from './app.component'

import 'hammerjs'
import { ChatRoomComponent } from './chat-room/chat-room.component'
import { PublicRoomComponent } from './public-room/public-room.component'
import { DirectMessagesComponent } from './direct-messages/direct-messages.component'
import { FriendsComponent } from './friends/friends.component'
import { HomeComponent } from './home/home.component'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { AppNavComponent } from './app-nav/app-nav.component'
import { LayoutModule } from '@angular/cdk/layout'
import { PublicChatHubComponent } from './public-chat-hub/public-chat-hub.component'

@NgModule({
  declarations: [
    AppComponent,
    ChatRoomComponent,
    PublicRoomComponent,
    DirectMessagesComponent,
    FriendsComponent,
    HomeComponent,
    AppNavComponent,
    PublicChatHubComponent,
  ],
  imports: [
    BrowserModule,
    AppMaterialModule,
    RouterModule.forRoot([
      { path: '', redirectTo: '/home', pathMatch: 'full' },
      { path: 'home', component: HomeComponent },
      { path: 'chat', component: ChatRoomComponent },
      { path: 'public', component: PublicChatHubComponent },
      { path: 'messages', component: DirectMessagesComponent },
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
