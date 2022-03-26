import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styles: [
  ]
})
export class HomeComponent implements OnInit {
  registerMode = false;

  constructor() { }

  ngOnInit(): void {
  }

  registerToggle(){
    this.registerMode = !this.registerMode;
  } 

  cancelRegister(event: boolean) {
    this.registerMode = event;
  }
}
