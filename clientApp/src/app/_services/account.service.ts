import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/User';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.dotnetUrl;
  private currentUserSource = new ReplaySubject<User>(1);   //private property 
  currentUser$ = this.currentUserSource.asObservable();     //observable

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);   //assinging user
        }
        return user;
      })
    )
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map((user: User) => {
        if (user) {
          this.setCurrentUser(user);
        }
        return user;
      })
    )
  }

  setCurrentUser(user: User) {
    if (user !== null) {
      const roles = this.getDecodedToken(user.token).role;

      user.roles = [];
      // checking if it is array then add it directy otherwise push to array
      Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    }
    localStorage.setItem('user.info', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user.info');
    this.currentUserSource.next(null);
  }

  getDecodedToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]));
  }
}
