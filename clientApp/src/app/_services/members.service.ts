import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';

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
  memberCache= new Map();   // map is like a dictionary in which we have keys and values

  constructor(private http: HttpClient) { }

  getMembers(userParams: UserParams){
    //getting data from cache
    var response = this.memberCache.get(Object.values(userParams).join('-'));
    if (response) return of(response);      

    
    let params = this.getPaginatedHeaders(userParams.pageNumber, userParams.pageSize);

    params = params.append('minAge', userParams.minAge.toString());
    params = params.append('maxAge', userParams.maxAge.toString());
    params = params.append('gender', userParams.gender);
    params = params.append('gender', userParams.orderBy);
    
    return this.getPaginatedResults<Member[]>(this.baseUrl + 'users', params)
      // sending data to cache
      .pipe(map(response => {
        this.memberCache.set(Object.values(userParams).join('-'), response);
        return response;
      }))
  }

  getMember(id: string){
    // combining different array from map to single array
    const member = [...this.memberCache.values()]         
      .reduce((prvArr,currElem) => prvArr.concat(currElem.results), [])
      .find((member: Member) => member.id.toString() === id);
    
    // returning member from singled array
    if (member) return of(member);        
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


  private getPaginatedResults<T>(url, params){
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();
    return this.http.get<T>(url, {observe: 'response', params}).pipe(
      map(response => {
        paginatedResult.results = response.body;
        if(response.headers.get('Pagination-Info') !== null){
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination-Info'));
        }
        return paginatedResult;
      })
    )
  }

  private getPaginatedHeaders(pageNumber: number, pageSize: number){
    let params = new HttpParams();  
    params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());
    
    return params;
  }
}
