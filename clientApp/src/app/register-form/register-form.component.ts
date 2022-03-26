import { Component, EventEmitter, OnInit, Output } from '@angular/core';
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

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  register() {
    this.accountService.register(this.model).subscribe(response => {
      console.log(response);
      this.cancel();
    }, err => {
      console.log(err);
    })    
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
