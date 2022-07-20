import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/User';
import { UserParams } from 'src/app/_models/userParams';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styles: [
  ]
})
export class MemberListComponent implements OnInit {
  members: Member[]
  pagination: Pagination;
  userParams: UserParams;
  user: User;

  genderList = [{ value: 'male', display: 'Males' }, { value: 'female', display: 'Females' }];

  constructor(private memberService: MembersService, 
    private router: Router) {    
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;   
    this.userParams = this.memberService.getUserParams();    
   } 

  ngOnInit(): void {        
    this.loadMembers();
  }

  loadMembers(){
    this.memberService.setUserParams(this.userParams);
    this.memberService.getMembers(this.userParams).subscribe((response => {
      this.members = response.results;
      this.pagination = response.pagination;
    }))
  }

  resetFilters(){
    this.userParams = this.memberService.resetUserParams();
    this.loadMembers();
  }

  pageChanged(event: any) {
    this.userParams.pageNumber = event.page;
    this.memberService.setUserParams(this.userParams); 
    this.loadMembers();
  }  
}