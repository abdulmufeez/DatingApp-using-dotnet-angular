import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Member } from '../_models/member';
import { UserProfileService } from '../_services/user-profile.service';

@Component({
  selector: 'app-user-profile-form',
  templateUrl: './user-profile-form.component.html',
  styles: [
  ]
})
export class UserProfileFormComponent implements OnInit {
  member: Member;
  addPhoto: boolean = false;  
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
    private toastr: ToastrService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.initilizeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  // reactive validate form
  initilizeForm() {
    this.registerForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      knownAs: ['', Validators.required],
      gender: ['male', Validators.required],
      dateOfBirth: ['', Validators.required],
      introduction: ['', Validators.required],
      lookingFor: ['', Validators.required],
      interests: ['', Validators.required],
      country: ['', Validators.required],
      city: ['', Validators.required]
    });
  }

  addProfile() {    
    this.userProfileService.addProfile(this.registerForm.value).subscribe(response => {
      this.member = response;
      this.toastr.success("Profile Created SuccessFully");
      this.addPhoto = true;
    }, err => {
      this.validationErrors = err;
      console.log(err);
    })
  }
}
