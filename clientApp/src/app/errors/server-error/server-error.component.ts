import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styles: [
  ]
})
export class ServerErrorComponent implements OnInit {
  error: any;

  constructor( private router: Router) {
    //getting the state from route which is made by the server
    const navigation = this.router.getCurrentNavigation();
    this.error = navigation?.extras?.state?.['error'];
   }

  ngOnInit(): void {
  }

}
