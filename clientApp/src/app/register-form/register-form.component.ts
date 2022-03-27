import { Component, EventEmitter, OnInit, Output } from '@angular/core';
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

  constructor(private accountService: AccountService, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  register() {
    this.accountService.register(this.model).subscribe(response => {
      this.toastr.success('Successfully registered');
      console.log(response);
      this.cancel();
    }, err => {
      this.toastr.error(err.error);
      console.log(err);
    })    
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
