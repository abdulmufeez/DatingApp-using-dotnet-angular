import { Component, OnInit } from '@angular/core';
import { User } from './_models/User';
import { AccountService } from './_services/account.service';
import { UserPresenceService } from './_services/user-presence.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styles: []
})
export class AppComponent implements OnInit {
  title = 'Dating App';  

  constructor(private accountService: AccountService, 
    private presenceHubService: UserPresenceService) {}

  ngOnInit(): void {
    //when angular start everything here is also started    
    this.setCurrentUser();    
  }

  setCurrentUser(){
    const user: User = JSON.parse(localStorage.getItem('user.info'));
    if (user) {
      this.accountService.setCurrentUser(user);       
      this.presenceHubService.createHubConnection(user); 
    }    
  }
    
}
