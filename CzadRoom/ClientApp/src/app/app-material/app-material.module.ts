import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import {
  MatInputModule, MatButtonModule, MatMenuModule, MatTabsModule, MatSidenavModule,
  MatListModule, MatToolbarModule, MatIconModule
} from '@angular/material'

@NgModule({
  imports: [
    CommonModule,
    MatButtonModule,
    MatInputModule,
    MatMenuModule,
    MatTabsModule,
    MatSidenavModule,
    MatListModule,
    MatToolbarModule,
    MatIconModule
  ],
  exports: [
    MatButtonModule,
    MatInputModule,
    MatMenuModule,
    MatTabsModule,
    MatSidenavModule,
    MatListModule,
    MatToolbarModule,
    MatIconModule
  ],
  declarations: []
})
export class AppMaterialModule { }
