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
    return this.http.post<User>(this.baseUrl + '/api/account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          localStorage.setItem('user.info', JSON.stringify(user));
          this.currentUserSource.next(user);    //assinging user
        }
        return user;
      })
    )
  }

  register(model: any){
    return this.http.post<User>(this.baseUrl + '/api/account/register', model).pipe(
      map ((user: User) =>{        
        if (user) {
          localStorage.setItem('user.info', JSON.stringify(user));
          this.currentUserSource.next(user);    
        }
        return user;
      })
    )
  }

  setCurrentUser(user: User) {
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user.info');
    this.currentUserSource.next(null);
  }
}
