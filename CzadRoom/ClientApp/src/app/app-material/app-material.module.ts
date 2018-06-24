import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatInputModule, MatButtonModule } from '@angular/material';

@NgModule({
  imports: [
    CommonModule,
    MatButtonModule,
    MatInputModule
  ],
  exports: [
    MatButtonModule,
    MatInputModule
  ],
  declarations: []
})
export class AppMaterialModule { }
