import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { map, Observable } from 'rxjs';
import { User } from '../_models/User';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private accountService: AccountService, private toastr: ToastrService, private router: Router) {}

  //this function check if user is loggedin or not
  canActivate(): Observable<boolean> {              
    return this.accountService.currentUser$.pipe(
      map((user: User) => {
        if (user) return true;
        else  {
          this.router.navigateByUrl('/');
          this.toastr.error('Login or Register yourself');
          return false;
        }        
      })
    )
  }
}
