import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { RolesModalComponent } from 'src/app/modals/roles-modal/roles-modal.component';
import { User } from 'src/app/_models/User';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css'],
  styles: [
  ]
})
export class UserManagementComponent implements OnInit {
  users: Partial<User[]>;
  bsModalRef: BsModalRef;

  constructor(private adminService: AdminService,
    private modalService: BsModalService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getUsersWithRoles();
  }

  getUsersWithRoles() {
    this.adminService.getUsersWithRole().subscribe(users => {
      this.users = users;
    })
  }

  openRolesModal(user: User) {
    const config = {
      class: 'modal-dialog-centered',
      initialState: {
        user,
        roles: this.getRolesArray(user)
      }
    }
    this.bsModalRef = this.modalService.show(RolesModalComponent, config);
    this.bsModalRef.content.updateSelectedRoles.subscribe(values => {
      const rolesToUpdate = {
        roles: [...values.filter(element => element.checked === true).map(element => element.name)]
      };
      if (rolesToUpdate) {
        this.adminService.updateUserRoles(user.id, rolesToUpdate.roles.toString()).subscribe(() => {
          user.roles = [...rolesToUpdate.roles]
          this.toastr.success('Suucessfully update roles of ' + user.username); //+ ' to ' + rolesToUpdate.roles.toString().split(',')[0] + ' and ' + rolesToUpdate.roles.toString().split(',')[1]);
        })
      }
    })
  }

  disableAccount(user: User, isDisabled: string){
    this.adminService.disableAccount(user.id, isDisabled).subscribe(() => {      
      this.getUsersWithRoles();
    })
  }

  private getRolesArray(user: User) {
    const roles = [];
    const userRoles = user.roles;
    // creating an array of object so that its hold name with its value
    const availableRoles: any[] = [
      { name: 'Admin', value: 'Admin' },
      { name: 'Moderator', value: 'Moderator' },
      { name: 'Member', value: 'Member' }
    ];

    availableRoles.forEach(role => {
      let isMatch = false;
      // checking if user role is matched then checked it
      for (const userRole of userRoles) {
        if (role.name === userRole) {
          isMatch = true;
          role.checked = true;
          roles.push(role);
          break;
        }
      }
      // if not matched then unchecked it so that we can update roles
      if (!isMatch) {
        role.checked = false;
        roles.push(role);
      }
    })
    return roles;
  }
}
