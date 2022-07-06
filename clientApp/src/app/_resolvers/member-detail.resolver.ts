import { Injectable } from '@angular/core';
import {
  Router, Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { Member } from '../_models/member';
import { MembersService } from '../_services/members.service';

@Injectable({
  providedIn: 'root'
})

// this router resolver will fetch data before the component loads, this will remove error of undefined
// router will automatically subscribe and unsub http request
export class MemberDetailResolver implements Resolve<Member> {
  
  constructor(private memberService: MembersService) {        }

  resolve(route: ActivatedRouteSnapshot): Observable<Member> {
    return this.memberService.getMember(route.paramMap.get('username'));
  }
}
