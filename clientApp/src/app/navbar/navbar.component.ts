import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../_models/User';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styles: [
  ]
})
export class NavbarComponent implements OnInit {
  model: any = {}   //initilize empty object with dynamic type

  constructor(public accountService: AccountService) { };

  ngOnInit(): void {

  }

  login(){
    this.accountService.login(this.model).subscribe(response => {
      console.log(response);      
    }, error => { 
      console.log(error);
      })
  }

  logout() {
    this.accountService.logout();      
  }  
}
