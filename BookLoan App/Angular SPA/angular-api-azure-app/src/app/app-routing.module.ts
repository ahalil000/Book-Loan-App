import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AboutComponent } from './about/about.component';
import { HomeComponent } from './home/home.component';
import { BooksComponent } from './books/books.component';
import { BookDetailComponent } from './book-detail/book-detail.component';
import { BookSearchComponent } from './book-search/book-search.component';
import { BookDetail2Component } from './book-detail-2/book-detail-2.component';
import { BookDetailComponentPageResolver } from './book-detail-2/book-detail-2.component.resolver';
import { BookDetail3Component } from './book-detail-3/book-detail-3.component';
import { AuthGuard } from './security/auth.guard';
import { LoginComponent } from './login/login.component';
import { BookViewComponent } from './book-view/book-view.component';
import { BookBorrowComponent } from './book-borrow/book-borrow.component';
// import { AccountUsersComponent } from './account-users/account-users.component';
// import { AccountUserNewComponent } from './account-user-new/account-user-new.component';
// import { AccountUserViewComponent } from './account-user-view/account-user-view.component';
// import { AccountUserEditComponent } from './account-user-edit/account-user-edit.component';
import { BookReturnComponent } from './book-return/book-return.component';
import { BookNewComponent } from './book-new/book-new.component';


const routes: Routes = [
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
  { path: '', redirectTo: 'home', pathMatch: 'prefix'},
  { path: 'login', component: LoginComponent },    
  { path: 'about', component: AboutComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: 'books', component: BooksComponent },
  { path: 'book-detail/:id', component: BookDetailComponent },
  { path: 'book-search', component: BookSearchComponent },
  { path: 'book-detail-2/:id', component: BookDetail2Component, resolve: { pageData: BookDetailComponentPageResolver } },
  { path: 'book-detail-3/:id', component: BookDetail3Component },
  { path: 'book-view/:id', component: BookViewComponent },  
  { path: 'book-new', component: BookNewComponent }, 
  { path: 'book-borrow/:id', component: BookBorrowComponent },
  { path: 'book-return/:id', component: BookReturnComponent },
  // { path: 'account-users', component: AccountUsersComponent, canActivate: [AuthGuard] },
  // { path: 'account-user-new', component: AccountUserNewComponent, canActivate: [AuthGuard] },
  // { path: 'account-user-edit/:id', component: AccountUserEditComponent, canActivate: [AuthGuard] },
  // { path: 'account-user-view/:id', component: AccountUserViewComponent, canActivate: [AuthGuard] },
  { path: 'accounts', loadChildren: () => import('./accounts/accounts.module').then(m => m.AccountsModule) }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
