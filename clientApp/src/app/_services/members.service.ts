import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';

//assigning token to http header
// const httpOptions = {
//   headers : new HttpHeaders({
//     Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user.info'))?.token
//   })
// }

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.dotnetUrl;

  constructor(private http: HttpClient) { }

  getMembers(){
    return this.http.get<Member[]>(this.baseUrl + 'users');
  }

  getMember(id: string){
    return this.http.get<Member>(this.baseUrl + 'users/' + id);
  }

  getMemberByAppId(appId: string){
    return this.http.get<Member>(this.baseUrl + 'users/edit/' + appId);
  }

  updateMember(member: Member){
    return this.http.put(this.baseUrl + 'users', member);
  }
}
