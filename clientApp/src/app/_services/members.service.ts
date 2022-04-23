import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
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
  members: Member[] = [];

  constructor(private http: HttpClient) { }

  getMembers(){
    if (this.members.length > 0) return of(this.members);
    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      map(members  => {
        this.members = members;
        return members;
      })
    );
  }

  getMember(id: string){
    const member = this.members.find(x => x.id.toString() === id);
    if (member !== undefined) return of(member);
    return this.http.get<Member>(this.baseUrl + 'users/' + id);
  }

  getMemberByAppId(appId: string){
    const member = this.members.find(x => x.applicationUserId.toString() === appId);
    if (member !== undefined) return of(member);
    return this.http.get<Member>(this.baseUrl + 'users/edit/' + appId);
  }

  updateMember(member: Member){
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map (() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }
}
