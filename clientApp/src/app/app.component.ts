import { Component, OnInit } from '@angular/core';
import { User } from './_models/User';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styles: []
})
export class AppComponent implements OnInit {
  title = 'Dating App';  

  constructor(private accountService: AccountService) {}

  ngOnInit(): void {
    //when angular start everything here is also started    
    this.setCurrentUser();    
  }

  setCurrentUser(){
    const user: User = JSON.parse(localStorage.getItem('user.info'));
    this.accountService.setCurrentUser(user);
  }
    
}
