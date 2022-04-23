import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FileUploadModule } from 'ng2-file-upload';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    FormsModule, //for forms in angular 
    BsDropdownModule.forRoot(),  //for dropdown
    ToastrModule.forRoot({      //toastr notifications
      positionClass:"toast-bottom-right"
    }),
    TabsModule.forRoot(), //for tabs
    NgxGalleryModule,    //for photos gallery
    NgxSpinnerModule,
    FileUploadModule
  ],
  exports: [
    BrowserAnimationsModule,
    FormsModule,
    BsDropdownModule,
    ToastrModule,
    TabsModule,
    NgxGalleryModule,
    NgxSpinnerModule,
    FileUploadModule
  ]
})
export class SharedModule { }
