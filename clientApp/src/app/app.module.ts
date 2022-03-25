import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {HttpClientModule} from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavbarComponent } from './navbar/navbar.component';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule, //for routing links
    HttpClientModule, //for api call
    BrowserAnimationsModule,
    FormsModule, //for forms in angular 
    BsDropdownModule.forRoot()  //for dropdown
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
