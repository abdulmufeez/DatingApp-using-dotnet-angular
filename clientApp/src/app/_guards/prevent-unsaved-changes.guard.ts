import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { UserProfileFormComponent } from '../user-profile-form/user-profile-form.component';

@Injectable({
  providedIn: 'root'
})

// Preventing component without saving 
export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> {
  canDeactivate(
    component: MemberEditComponent): boolean {
      if (component.editForm.dirty) {
        return confirm('Are you sure you want to continue? Any unsaved chages will be lost');
      }    
    return true;
  }  
}


export class PreventUnsavedChangesGuardForAddProfile implements CanDeactivate<unknown> {
  canDeactivate(
    component: UserProfileFormComponent): boolean {
      if (component.addProfileForm.dirty) {
        return confirm('Are you sure you want to continue? Any unsaved chages will be lost');
      }    
    return true;
  }  
}