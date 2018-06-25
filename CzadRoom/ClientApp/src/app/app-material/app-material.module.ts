import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { MatInputModule, MatButtonModule, MatMenuModule, MatTabsModule, MatSidenavModule, MatListModule} from '@angular/material'

@NgModule({
  imports: [
    CommonModule,
    MatButtonModule,
    MatInputModule,
    MatMenuModule,
    MatTabsModule,
    MatSidenavModule,
    MatListModule
  ],
  exports: [
    MatButtonModule,
    MatInputModule,
    MatMenuModule,
    MatTabsModule,
    MatSidenavModule,
    MatListModule
  ],
  declarations: []
})
export class AppMaterialModule { }
