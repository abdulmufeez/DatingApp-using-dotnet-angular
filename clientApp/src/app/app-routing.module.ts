import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { AdminGuard } from './_guards/admin.guard';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {
    //this object is created so that guards is applied to all routes
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    // if i enable this my application become unresponsive
    //canDeactivate: [PreventUnsavedChangesGuardForAddProfile],
    children: [
      {path: 'members', component: MemberListComponent},
      {path: 'member/edit', component: MemberEditComponent, pathMatch: 'full', canDeactivate: [PreventUnsavedChangesGuard]},
      {path: 'member/:username', component: MemberDetailComponent, pathMatch: 'full', resolve: {member: MemberDetailResolver}},
      {path: 'lists', component: ListsComponent},
      {path: 'messages', component: MessagesComponent},
      {path: 'admin', component: AdminPanelComponent, canActivate: [AdminGuard]}
    ]
  },  
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: '**', component: NotFoundComponent, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
