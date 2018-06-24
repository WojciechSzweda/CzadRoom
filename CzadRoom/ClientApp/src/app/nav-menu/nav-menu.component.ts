import { Component, OnInit, Inject } from '@angular/core'

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  links: Object[]
  activeLink: Object

  constructor() {
    this.links = [
      { name: 'Home', path: '' },
      { name: 'Chat Room', path: 'chat' },
      { name: 'Public Room', path: 'public' },
      { name: 'Direct Messages', path: 'messages' },
      { name: 'Friends', path: 'friends' }
    ]
    this.activeLink = this.links[0]
  }

  ngOnInit() {
  }

}
