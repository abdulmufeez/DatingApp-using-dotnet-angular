import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styles: [
  ]
})
export class HomeComponent implements OnInit, OnDestroy {
  registerMode = false;
  loggedIn = false;

  constructor(private accountService: AccountService,
    private router: Router) {}  

  ngOnInit(): void {
    // this is used to implements refresh functionality
    this.router.onSameUrlNavigation = 'reload';
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      if(user)
      {
        this.loggedIn = true;
      }
    })
  }

  ngOnDestroy(): void {
    this.loggedIn = false;    
  }

  registerToggle(){
    this.registerMode = !this.registerMode;
  } 

  cancelRegister(event: boolean) {
    this.registerMode = event;
    this.loggedIn = false;
  }
}
