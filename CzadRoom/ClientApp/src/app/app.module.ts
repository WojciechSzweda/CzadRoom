import { BrowserModule } from '@angular/platform-browser'
import { NgModule } from '@angular/core'
import { RouterModule } from '@angular/router'
import { AppMaterialModule } from './app-material/app-material.module'

import { AppComponent } from './app.component'
import { NavMenuComponent } from './nav-menu/nav-menu.component'

import 'hammerjs'
import { ChatRoomComponent } from './chat-room/chat-room.component'
import { PublicRoomComponent } from './public-room/public-room.component'
import { DirectMessagesComponent } from './direct-messages/direct-messages.component'
import { FriendsComponent } from './friends/friends.component'
import { HomeComponent } from './home/home.component'

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    ChatRoomComponent,
    PublicRoomComponent,
    DirectMessagesComponent,
    FriendsComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppMaterialModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent },
      { path: 'chat', component: ChatRoomComponent },
      { path: 'public', component: PublicRoomComponent },
      { path: 'messages', component: DirectMessagesComponent },
      { path: 'friends', component: FriendsComponent },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
