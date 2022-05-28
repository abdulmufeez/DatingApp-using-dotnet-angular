import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/User';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css'],
  styles: [
  ]
})
export class MemberEditComponent implements OnInit {
  // accessing Form from html/DOM
  @ViewChild('editForm') editForm: NgForm;
  user: User;
  member: Member;

  // preventing any action without saving data
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private accountService: AccountService, 
      private memberService: MembersService,
      private toastr: ToastrService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    this.loadMember();  
    this.editForm.controls     
  }
  
  loadMember(){
    this.memberService.getMemberByAppId(this.user.id.toString()).subscribe(member => { 
      this.member = member;      
    })
  }

  updateMember(){
    this.memberService.updateMember(this.member).subscribe(() => {
      this.toastr.success('Profile Edit Successfully');
      this.editForm.reset(this.member);
    });    
  }
}
