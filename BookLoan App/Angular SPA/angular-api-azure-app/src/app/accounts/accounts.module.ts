import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../material/material.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ComponentsModule } from '../components/components.module';
import { ValidatorsModule } from '../validators/validators.module';

import { AccountsRoutingModule } from './accounts-routing.module';
import { AccountUserEditComponent } from './account-user-edit/account-user-edit.component';
import { AccountUserNewComponent } from './account-user-new/account-user-new.component';
import { AccountUsersComponent } from './account-users/account-users.component';
import { AccountUserViewComponent } from './account-user-view/account-user-view.component';


@NgModule({
  declarations: [
    AccountUserEditComponent,
    AccountUserNewComponent,
    AccountUsersComponent,
    AccountUserViewComponent,
  ],
  imports: [
    CommonModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    ComponentsModule,
    ValidatorsModule,
    AccountsRoutingModule
  ]
})
export class AccountsModule { }
