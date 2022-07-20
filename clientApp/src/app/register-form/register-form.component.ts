import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
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
  addProfile: boolean = false;
  registerForm: FormGroup;
  validationErrors: string[] = [];

  constructor(private accountService: AccountService,
    private toastr: ToastrService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.initilizeForm();
  }

  // reactive validate form
  initilizeForm() {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(16)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value ? null : { matched: true }
    }
  }

  register() {
    this.accountService.register(this.registerForm.value).subscribe(() => {
      this.toastr.success('Successfully registered');
      this.addProfile = true;
    }, err => {
      this.validationErrors = err;
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
