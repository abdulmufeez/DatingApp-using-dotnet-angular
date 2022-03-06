import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styles: []
})
export class AppComponent implements OnInit {
  title = 'Dating App';
  users: any;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getUsers();
  }



  user :any;
  getUsers(){
    this.http.get(environment.dotnetUrl.toString() + '/api/users').subscribe(response => {
      this.users = response;
      console.log(response);
    }, error => {
      console.log(error);
    })
  }
}
