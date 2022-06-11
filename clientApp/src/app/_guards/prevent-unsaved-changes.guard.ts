import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { UserProfileFormComponent } from '../user-profile-form/user-profile-form.component';
import { ConfirmWindowService } from '../_services/confirm-window.service';

@Injectable({
  providedIn: 'root'
})

// Preventing component without saving 
export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> {
  
  constructor(private confirmWindowService: ConfirmWindowService) {}

  canDeactivate(
    component: MemberEditComponent): Observable<boolean> | boolean {
      if (component.editForm.dirty) {
        // instead of return js confirm dialog we can return our confirm modal
        // return confirm('Are you sure you want to continue? Any unsaved chages will be lost');

        this.confirmWindowService.confirm();
      }    
    return true;
  }  
}