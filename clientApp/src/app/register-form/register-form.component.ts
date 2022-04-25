import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styles: [
  ]
})
export class RegisterFormComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();    //used for sending data to parent component
  model : any = {};
  registerForm: FormGroup;

  constructor(private accountService: AccountService, 
    private toastr: ToastrService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.initilizeForm();
  }

  // reactive validate form
  initilizeForm(){
    this.registerForm = this.fb.group({
      username: ['', Validators.required],      
      email: ['', Validators.required],      
      password: ['', [Validators.required,Validators.minLength(6), Validators.maxLength(16)]],      
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]      
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value ? null : {matched: true}
    }
  }

  register() {
    console.log(this.registerForm.value);

    // this.accountService.register(this.model).subscribe(response => {
    //   this.toastr.success('Successfully registered');
    //   console.log(response);
    //   this.cancel();
    // }, err => {
    //   this.toastr.error(err.error);
    //   console.log(err);
    // })    
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
