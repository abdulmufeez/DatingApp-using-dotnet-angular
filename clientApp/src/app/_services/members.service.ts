import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/pagination';

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
  paginatedResult: PaginatedResult<Member[]> = new PaginatedResult();

  constructor(private http: HttpClient) { }

  getMembers(page?: number, itemsPerPage?:number)
  {
    let params = new HttpParams();
    if (page !== null && itemsPerPage !== null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }
    
    return this.http.get<Member[]>(this.baseUrl + 'users', {observe: 'response', params}).pipe(
      map(response => {
        this.paginatedResult.results = response.body;
        if(response.headers.get('Pagination-Info') !== null){
          this.paginatedResult.pagination = JSON.parse(response.headers.get('Pagination-Info'));
        }
        return this.paginatedResult;
      })
    )
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

  setMainPhoto(photoId: number){
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number){
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId)
  }
}
