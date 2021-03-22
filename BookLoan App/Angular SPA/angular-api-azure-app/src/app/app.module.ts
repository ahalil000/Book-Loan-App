import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MenuComponent } from './menu/menu.component';
import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material/material.module';
import { AboutComponent } from './about/about.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ImagebannerComponent } from './imagebanner/imagebanner.component';
import { HomeComponent } from './home/home.component';
import { BooksComponent } from './books/books.component';
import { BookDetailComponent } from './book-detail/book-detail.component';
import { ApiService } from './services/api.service';
import { LookupService } from './services/lookup.service';
import { BookSearchComponent } from './book-search/book-search.component';
import { BookDetail2Component } from './book-detail-2/book-detail-2.component';
import { BookDetailComponentPageResolver } from './book-detail-2/book-detail-2.component.resolver';
import { BookDetail3Component } from './book-detail-3/book-detail-3.component';
import { BookViewComponent } from './book-view/book-view.component';
import { BookNewComponent } from './book-new/book-new.component';
import { BookBorrowComponent } from './book-borrow/book-borrow.component';
import { LoginComponent } from './login/login.component';
import { AuthService } from './security/auth.service';
import { AuthInterceptor } from './security/auth.interceptor';
// import { AccountUsersComponent } from './account-users/account-users.component';
// import { AccountUserNewComponent } from './account-user-new/account-user-new.component';
// import { AccountUserViewComponent } from './account-user-view/account-user-view.component';
import { NotificationService } from './services/notification-service';
import { BookReturnComponent } from './book-return/book-return.component';
// import { AccountUserEditComponent } from './account-user-edit/account-user-edit.component';
//import { ForbiddenDateOfBirthDirective } from './validators/invalid-date-of-birth.diirective';
import { ComponentsModule } from './components/components.module';
import { ValidatorsModule } from './validators/validators.module';
import { APP_CONFIG, BOOKS_DI_CONFIG } from './app.config';
// import { CheckboxGroupComponent } from './components/checkbox-group/checkbox-group.component';
// import { SingleSelectGroupComponent } from './components/single-select-group/single-select-group.component';

@NgModule({
  declarations: [
    AppComponent,
    MenuComponent,
    AboutComponent,
    LoginComponent,
    // AccountUsersComponent,
    // AccountUserNewComponent,
    // AccountUserEditComponent,
    // AccountUserViewComponent,
    DashboardComponent,
    ImagebannerComponent,
    HomeComponent,
    BooksComponent,
    BookViewComponent,
    BookDetailComponent,
    BookDetail2Component,
    BookDetail3Component,
    BookSearchComponent,
    BookBorrowComponent,
    BookReturnComponent,
    //ForbiddenDateOfBirthDirective,
    BookNewComponent,
    //CheckboxGroupComponent,
    //SingleSelectGroupComponent
  ],
  imports: [
    NgbModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    NoopAnimationsModule,
    MaterialModule,
    ComponentsModule,
    ValidatorsModule
  ],
  // exports:
  // [
  //   CheckboxGroupComponent,
  //   SingleSelectGroupComponent
  // ],
  providers: [
    ApiService, 
    {
      provide: APP_CONFIG, 
      useValue: BOOKS_DI_CONFIG
    },
    NotificationService, 
    AuthService,
    {
        provide: HTTP_INTERCEPTORS,
        useClass: AuthInterceptor,
        multi: true
    },
    LookupService, 
    BookDetailComponentPageResolver
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
