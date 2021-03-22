import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../material/material.module';

//import { BrowserModule } from '@angular/platform-browser';
//import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
//import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';

import { CheckboxGroupComponent } from '../components/checkbox-group/checkbox-group.component';
//import { ForbiddenDateOfBirthDirective } from './validators/invalid-date-of-birth.diirective';
import { SingleSelectGroupComponent } from '../components/single-select-group/single-select-group.component';
import { PaginationComponent } from '../components/pagination/pagination.component';


//import { AccountsRoutingModule } from './accounts-routing.module';
//import { AccountUserEditComponent } from './account-user-edit/account-user-edit.component';
//import { AccountUserNewComponent } from './account-user-new/account-user-new.component';
//import { AccountUsersComponent } from './account-users/account-users.component';
//import { AccountUserViewComponent } from './account-user-view/account-user-view.component';


@NgModule({
  declarations: [
    //AccountUserEditComponent,
    //AccountUserNewComponent,
    //AccountUsersComponent,
    //AccountUserViewComponent,
    CheckboxGroupComponent,
    SingleSelectGroupComponent,
    PaginationComponent
  ],
  imports: [
    CommonModule,
    // NgbModule,
    // HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    // BrowserModule,
    // AppRoutingModule,
    // BrowserAnimationsModule,
    // NoopAnimationsModule,
    MaterialModule
    //AccountsRoutingModule
  ],
  exports: 
  [
    CheckboxGroupComponent,
    SingleSelectGroupComponent,
    PaginationComponent
  ]
})
export class ComponentsModule { }
