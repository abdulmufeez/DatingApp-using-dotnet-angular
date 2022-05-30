import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { getPaginatedHeaders, getPaginatedResults } from '../_helpers/paginationHelper';
import { LikeParams } from '../_models/likeParams';
import { Member } from '../_models/member';
import { User } from '../_models/User';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';

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
  user: User;
  userParams: UserParams;
  likeParams: LikeParams;
  memberCache = new Map();   // map is like a dictionary in which we have keys and values  

  constructor(private http: HttpClient, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
      this.userParams = new UserParams(user);
    });
    this.likeParams = new LikeParams();
  }

  getMembers(userParams: UserParams) {
    //getting data from cache
    var response = this.memberCache.get(Object.values(userParams).join('-'));
    if (response) return of(response);

    let params = getPaginatedHeaders(userParams.pageNumber, userParams.pageSize);

    params = params.append('minAge', userParams.minAge.toString());
    params = params.append('maxAge', userParams.maxAge.toString());
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);

    return getPaginatedResults<Member[]>(this.baseUrl + 'users', params, this.http)
      // sending data to cache
      .pipe(map(response => {
        this.memberCache.set(Object.values(userParams).join('-'), response);
        return response;
      }))
  }

  getMember(id: string) {
    // combining different array from map(cache) to single array
    const member = [...this.memberCache.values()]
      .reduce((prvArr, currElem) => prvArr.concat(currElem.results), [])
      .find((member: Member) => member.id.toString() === id);

    // returning member from singled array
    if (member) return of(member);
    return this.http.get<Member>(this.baseUrl + 'users/' + id);
  }

  getMemberByAppId(appId: string) {
    // const member = this.members.find(x => x.applicationUserId.toString() === appId);
    // if (member !== undefined) return of(member);     

    return this.http.get<Member>(this.baseUrl + 'users/edit/' + appId);
  }

  updateMember(member: Member) {
    // return this.http.put(this.baseUrl + 'users', member).pipe(
    //   map (() => {
    //     const index = this.members.indexOf(member);
    //     this.members[index] = member;
    //   })
    // );
    return this.http.put(this.baseUrl + 'users', member);
  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }

  addLike(id: number) {
    return this.http.post(this.baseUrl + 'likes/' + id.toString(), {});
  }

  getLikes(likeParams: LikeParams) {
    let params = getPaginatedHeaders(likeParams.pageNumber, likeParams.pageSize);
    params = params.append('predicate', likeParams.predicate);
    return getPaginatedResults<Partial<Member[]>>(this.baseUrl + 'likes/', params, this.http);
  }

  

  // helper methods
  getUserParams() {
    return this.userParams;
  }

  setUserParams(params: UserParams) {
    this.userParams = params;
  }

  resetUserParams() {
    this.userParams = new UserParams(this.user);
    return this.userParams;
  }

  getLikeParams() {
    return this.likeParams;
  }

  setLikeParams(params: LikeParams) {
    this.likeParams = params;
  }
}
