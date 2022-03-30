import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    FormsModule, //for forms in angular 
    BsDropdownModule.forRoot(),  //for dropdown
    ToastrModule.forRoot({
      positionClass:"toast-bottom-right"
    })
  ],
  exports: [
    BrowserAnimationsModule,
    FormsModule,
    BsDropdownModule,
    ToastrModule
  ]
})
export class SharedModule { }
