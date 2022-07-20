import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from '../_models/member';
import { User } from '../_models/User';
import { AccountService } from '../_services/account.service';
import { UserPresenceService } from '../_services/user-presence.service';
import { UserProfileService } from '../_services/user-profile.service';

@Component({
  selector: 'app-user-profile-form',
  templateUrl: './user-profile-form.component.html',
  styles: [
  ]
})
export class UserProfileFormComponent implements OnInit {  
  user : User;
  registerForm: FormGroup;
  validationErrors: string[] = [];
  maxDate: Date;

  @ViewChild('addProfileForm') addProfileForm: NgForm;
  // preventing any action without saving data
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.addProfileForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private userProfileService: UserProfileService,
    private accountService: AccountService,
    private toastr: ToastrService,
    private fb: FormBuilder,
    private router: Router) {
      this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user)
    }

  ngOnInit(): void {    
    this.initilizeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  // reactive validate form
  initilizeForm() {       
    this.registerForm = this.fb.group({
      firstName: [''],
      lastName: ['', Validators.required],
      knownAs: ['', Validators.required],
      gender: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      introduction: [''],
      lookingFor: [''],
      interests: [''],
      country: ['', Validators.required],
      city: ['', Validators.required],
      applicationUserId: []      
    });
  }

  addProfile() {
    this.registerForm.patchValue({applicationUserId: this.user.id.toString()});    
    this.userProfileService.addProfile(this.registerForm.value).subscribe(() => {      
      this.toastr.success("Profile Created SuccessFully");      
           
      // this.accountService.logout();
      // this.toastr.info("You can now log in");
      this.router.navigateByUrl('/member/edit');
    }, err => {
      this.validationErrors = err;      
    })
  }
}
