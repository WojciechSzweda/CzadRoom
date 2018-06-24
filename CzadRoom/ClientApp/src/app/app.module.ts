import { BrowserModule } from '@angular/platform-browser'
import { NgModule } from '@angular/core'
import {RouterModule} from '@angular/router'
import {AppMaterialModule} from './app-material/app-material.module'

import { AppComponent } from './app.component'
import { NavMenuComponent } from './nav-menu/nav-menu.component'

import 'hammerjs'

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent
  ],
  imports: [
    BrowserModule,
    AppMaterialModule,
    RouterModule.forRoot([
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
