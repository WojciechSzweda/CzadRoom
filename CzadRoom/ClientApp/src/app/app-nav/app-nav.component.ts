import { Component } from '@angular/core'
import { BreakpointObserver, Breakpoints, BreakpointState } from '@angular/cdk/layout'
import { Observable } from 'rxjs'
import { map } from 'rxjs/operators'

@Component({
  selector: 'app-nav',
  templateUrl: './app-nav.component.html',
  styleUrls: ['./app-nav.component.css']
})
export class AppNavComponent {

  links: Object[]
  activeLink: Object
  isMobileMode: boolean
  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches)
    )

  constructor(private breakpointObserver: BreakpointObserver) {
    this.links = [
      { name: 'Home', path: '' },
      { name: 'Chat Room', path: 'chat' },
      { name: 'Public Rooms Hub', path: 'public' },
      { name: 'Direct Messages', path: 'messages' },
      { name: 'Friends', path: 'friends' }
    ]
    this.activeLink = this.links[0]
    this.isHandset$.subscribe(x => this.isMobileMode = x)
  }

  public closeDrawer(drawerCallback: any): void {
    if (this.isHandset$) {
      drawerCallback()
    }
  }
}
