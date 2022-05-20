import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { MembersService } from './members.service';

@Injectable({
  providedIn: 'root'
})
export class UserProfileService {
  baseUrl = environment.dotnetUrl;

  constructor(private http: HttpClient, private memberService: MembersService) { }

  addProfile(member: Member){
    return this.http.post<Member>(this.baseUrl + 'users/add-profile', member)
  }
}
