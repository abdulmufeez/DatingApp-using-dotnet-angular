import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/User';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styles: [
  ]
})
export class UserManagementComponent implements OnInit {
  users: Partial<User[]>;

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.getUsersWithRoles();
  }

  getUsersWithRoles(){
    this.adminService.getUsersWithRole().subscribe(users => {
      this.users = users;
    })
  }
}
