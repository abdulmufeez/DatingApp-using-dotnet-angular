import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { map, Observable } from 'rxjs';
import { User } from '../_models/User';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(private accountService: AccountService, private toastr: ToastrService) {}
  
  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map((user: User) => {
        if (user.roles.includes('Admin') || user.roles.includes('Moderator')) {
          return true;
        }
        this.toastr.error('You cannot enter in admin panel')
        return false;
      })
    )
  } 
}
