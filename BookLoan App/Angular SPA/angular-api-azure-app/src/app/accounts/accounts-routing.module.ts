import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '../security/auth.guard';

import { AccountUsersComponent } from './account-users/account-users.component';
import { AccountUserNewComponent } from './account-user-new/account-user-new.component';
import { AccountUserViewComponent } from './account-user-view/account-user-view.component';
import { AccountUserEditComponent } from './account-user-edit/account-user-edit.component';

const routes: Routes = 
[
  { path: '', component: AccountUsersComponent, canActivate: [AuthGuard] },
  { path: 'account-users', component: AccountUsersComponent, canActivate: [AuthGuard] },
  { path: 'account-user-new', component: AccountUserNewComponent, canActivate: [AuthGuard] },
  { path: 'account-user-edit/:id', component: AccountUserEditComponent, canActivate: [AuthGuard] },
  { path: 'account-user-view/:id', component: AccountUserViewComponent, canActivate: [AuthGuard] }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountsRoutingModule { }
