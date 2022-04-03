import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { HomeComponent } from '../home/home.component';
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

  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService) { };

  ngOnInit(): void {

  } 

  registerToggle(){  
    this.router.navigateByUrl('/register-form');
  }

  login(){
    this.accountService.login(this.model).subscribe(response => {
      this.toastr.success('Login Successfully');
      this.router.navigateByUrl('/members');     
    })
  }

  logout() {
    this.accountService.logout();      
    this.router.navigateByUrl('/');     
  }  
}
