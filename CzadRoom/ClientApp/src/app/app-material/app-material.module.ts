import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { MatInputModule, MatButtonModule, MatMenuModule, MatTabsModule} from '@angular/material'

@NgModule({
  imports: [
    CommonModule,
    MatButtonModule,
    MatInputModule,
    MatMenuModule,
    MatTabsModule
  ],
  exports: [
    MatButtonModule,
    MatInputModule,
    MatMenuModule,
    MatTabsModule
  ],
  declarations: []
})
export class AppMaterialModule { }
