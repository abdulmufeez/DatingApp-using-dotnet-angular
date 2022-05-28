import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../_models/User';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.dotnetUrl;

  constructor(private http: HttpClient) { }

  getUsersWithRole() {
    return this.http.get<Partial<User[]>>(this.baseUrl + 'admin/users-with-roles');
  }

  updateUserRoles(userId: number, roles: string){
    return this.http.post(this.baseUrl + 'admin/edit-roles/' + userId.toString() + '?roles=' + roles, {});
  }

  disableAccount(userId: number, isDisabled: string){
    return this.http.put(this.baseUrl + 'admin/disable-account/' + userId + '?isDisabled=' + isDisabled, {});
  }
}
